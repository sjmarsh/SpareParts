using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;
using SpareParts.Shared.Models;
using SpareParts.Test.Helpers;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class InventoryTests : IAsyncLifetime
    {
        private readonly InventoryPage _inventoryPage;
        private readonly LoginPage _loginPage;
        private readonly SparePartsDbContext _dbContext;
        private readonly DataHelper _dataHelper;

        public InventoryTests(SparePartsBrowserTestFixture fixture)
        {
            _inventoryPage = fixture.Pages.Inventory;
            _loginPage = fixture.Pages.Login;
            _dbContext = fixture.DbContext;
            _dataHelper = new DataHelper(_dbContext);
        }

        public async Task InitializeAsync()
        {
            _dbContext.InventoryItems.RemoveRange(_dbContext.InventoryItems);
            await _dbContext.SaveChangesAsync();
            _dbContext.Parts.RemoveRange(_dbContext.Parts);
            await _dbContext.SaveChangesAsync();
                        
            await _inventoryPage.InitializePage();
        }

        public async Task DisposeAsync()
        {
            await Task.Run(() =>
            {
            });
        }

        [Fact]
        public async Task Should_HaveExpectedHeader()
        {
            (await _inventoryPage.PageHeader()).Should().Be("Inventory");
        }


        [Fact]
        public async Task Should_DefaultToFirstTab()
        {
            (await _inventoryPage.SelectedTabIndex()).Should().Be(0);
        }

        [Fact]
        public async Task Should_EnterManualStockItem()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(2);
            var part2 = parts[1];
            await _inventoryPage.InitializePage();
            const int quantity = 3;

            await _inventoryPage.SelectTabNumber(0);
            var manualStockEntry = _inventoryPage.ManualStockEntry;
            await manualStockEntry.SelectPart(part2.Name!);
            await manualStockEntry.EnterQuantity(quantity);
            await manualStockEntry.SaveManualStock();

            _dbContext.InventoryItems.Count().Should().Be(1);
            var inventoryItem = _dbContext.InventoryItems.First();
            inventoryItem.PartID.Should().Be(part2.ID);
            inventoryItem.Quantity.Should().Be(quantity);
        }

        [Fact]
        public async Task Should_EnterStocktakeItems()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(2);
            var part1 = parts[0];
            var part2 = parts[1];
            _dbContext.InventoryItems.Add(new API.Entities.InventoryItem { PartID = part1.ID, Quantity = 1, DateRecorded = DateTime.Now });
            _dbContext.InventoryItems.Add(new API.Entities.InventoryItem { PartID = part2.ID, Quantity = 1, DateRecorded = DateTime.Now });
            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
            await _inventoryPage.InitializePage();
            const int part1Qty = 33;
            const int part2Qty = 45;

            await _inventoryPage.SelectTabNumber(1);
            var stocktake = _inventoryPage.Stocktake;
            await stocktake.EnterQuantityFor(part1.Name!, part1Qty);
            await stocktake.EnterQuantityFor(part2.Name!, part2Qty);
            await stocktake.SubmitStocktake();

            _dbContext.InventoryItems.Count().Should().Be(4);
            var inventoryItem1 = _dbContext.InventoryItems.Where(i => i.PartID == part1.ID).OrderBy(o => o.DateRecorded).Last();
            inventoryItem1.PartID.Should().Be(part1.ID);
            inventoryItem1.Quantity.Should().Be(part1Qty);
            var inventoryItem2 = _dbContext.InventoryItems.Where(i => i.PartID == part2.ID).OrderBy(o => o.DateRecorded).Last();
            inventoryItem2.PartID.Should().Be(part2.ID);
            inventoryItem2.Quantity.Should().Be(part2Qty);
        }

        [Fact]
        public async Task Should_DisplayCurrentStock()
        {
            var inventoryItems = (await _dataHelper.CreateInventoryItemDetailListInDatabase(3)).OrderBy(i => i.PartName).ToList();  // creates all current records by default

            await _inventoryPage.SelectTabNumber(2);
            var currentStock = _inventoryPage.CurrentStock;

            ItemsShouldBeEquivalent(inventoryItems[0], await currentStock.GetItemForRow(0));
            ItemsShouldBeEquivalent(inventoryItems[1], await currentStock.GetItemForRow(1));
            ItemsShouldBeEquivalent(inventoryItems[2], await currentStock.GetItemForRow(2));
        }

        [Fact]
        public async Task Should_DisplayInventoryHistory()
        {
            var parts = (await _dataHelper.CreatePartListInDatabase(3)).OrderBy(p => p.Name).ToList();
            var quantities = new[] { 1, 2, 3, 4, 5, 6 };
            await _inventoryPage.InitializePage();
            await _inventoryPage.SelectTabNumber(0);
            var manualStockEntry = _inventoryPage.ManualStockEntry;           
            var partIndex = 0;
            for (int i = 0; i < quantities.Length; i++)
            {               
                if(partIndex == 3)
                {
                    partIndex = 0;
                }
                await manualStockEntry.SelectPart(parts[partIndex].Name!);
                await manualStockEntry.EnterQuantity(quantities[i]);
                await manualStockEntry.SaveManualStock();
                partIndex++;
            }

            await _inventoryPage.SelectTabNumber(3);
            var history = _inventoryPage.History;

            // order by part name then, time recorded descending
            var historyRow0 = await history.GetItemForRow(0);
            historyRow0.PartName.Should().Be(parts[0].Name);
            historyRow0.Quantity.Should().Be(quantities[3]);

            var historyRow1 = await history.GetItemForRow(1);
            historyRow1.PartName.Should().Be(parts[0].Name);
            historyRow1.Quantity.Should().Be(quantities[0]);

            var historyRow2 = await history.GetItemForRow(2);
            historyRow2.PartName.Should().Be(parts[1].Name);
            historyRow2.Quantity.Should().Be(quantities[4]);

            var historyRow3 = await history.GetItemForRow(3);
            historyRow3.PartName.Should().Be(parts[1].Name);
            historyRow3.Quantity.Should().Be(quantities[1]);

            var historyRow4 = await history.GetItemForRow(4);
            historyRow4.PartName.Should().Be(parts[2].Name);
            historyRow4.Quantity.Should().Be(quantities[5]);

            var historyRow5 = await history.GetItemForRow(5);
            historyRow5.PartName.Should().Be(parts[2].Name);
            historyRow5.Quantity.Should().Be(quantities[2]);
        }

        private void ItemsShouldBeEquivalent(InventoryItemDetail actual, InventoryItemDetail expected)
        {
            actual.PartName.Should().Be(expected.PartName);
            actual.Quantity.Should().Be(expected.Quantity);
            actual.DateRecorded.Date.Should().Be(expected.DateRecorded.Date);
        }
    }
}
