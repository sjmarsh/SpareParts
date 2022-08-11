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
        private readonly SparePartsDbContext _dbContext;
        private readonly DataHelper _dataHelper;

        public InventoryTests(SparePartsBrowserTestFixture fixture)
        {
            _inventoryPage = fixture.Pages.Inventory;
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
            await manualStockEntry.SelectPart(part2.Name);
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

        private void ItemsShouldBeEquivalent(InventoryItemDetail actual, InventoryItemDetail expected)
        {
            actual.PartName.Should().Be(expected.PartName);
            actual.Quantity.Should().Be(expected.Quantity);
            actual.DateRecorded.Date.Should().Be(expected.DateRecorded.Date);
        }
    }
}
