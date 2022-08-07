using SpareParts.Shared.Models;
using SpareParts.Test.Helpers;

namespace SpareParts.API.Int.Tests
{
    [Collection("Spare Parts Tests")]
    public class InventoryTests
    {
        private readonly SparePartsTestFixture _testFixture;
        private readonly DataHelper _dataHelper;

        public InventoryTests(SparePartsTestFixture testFixture)
        {
            _testFixture = testFixture;
            _dataHelper = new DataHelper(_testFixture.DbContext);
            
            // clear inventory tables between tests
            _testFixture.DbContext.InventoryItems.RemoveRange(_testFixture.DbContext.InventoryItems);
            _testFixture.DbContext.SaveChanges();
        }

        [Fact]
        public async Task Get_Should_ReturnRequiredInventoryItem()
        {
            var savedItem = await _dataHelper.CreateInventoryItemInDatabase();
            savedItem.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.GetRequest<InventoryItemResponse>($"/api/inventory/?id={savedItem.ID}");

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(savedItem);
        }

        [Fact]
        public async Task GetIndex_Should_ReturnListOfInventoryItems()
        {
            var items = await _dataHelper.CreateInventoryItemListInDatabase(5);

            var result = await _testFixture.GetRequest<InventoryItemListResponse>($"/api/inventory/index");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.Items.Should().BeEquivalentTo(items);
        }
                
        [Fact]
        public async Task GetIndexDetail_Should_ReturnListOfInventoryItemsWithPartName()
        {
            var items = await _dataHelper.CreateInventoryItemListInDatabase(5);

            var result = await _testFixture.GetRequest<InventoryItemDetailListResponse>($"/api/inventory/index-detail");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.Items.Should().AllSatisfy(i => i.PartName.Should().NotBeNullOrEmpty());
        }

        [Fact]
        public async Task GetIndexDetail_Should_ReturnListOfItemsWithRequiredPaging()
        {
            await _dataHelper.CreateInventoryItemListInDatabase(5);

            var result = await _testFixture.GetRequest<InventoryItemDetailListResponse>($"/api/inventory/index-detail?skip=2&take=2");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetIndexDetail_Should_ReturnListOfCurrentItems()
        {
            var parts = _dataHelper.GetPartsFakerConfig().Generate(3);
            parts[0].StartDate = DateTime.Today.AddYears(-1); // current
            parts[0].EndDate = null;
            parts[1].StartDate = DateTime.Today.AddYears(-1); // not current
            parts[1].EndDate = DateTime.Today.AddDays(-2);
            parts[2].StartDate = DateTime.Today.AddYears(1); // not current
            parts[2].EndDate = DateTime.Today.AddDays(2);

            foreach (var part in parts)
            {
                _testFixture.DbContext.Parts.Add(part);
            }
            await _testFixture.DbContext.SaveChangesAsync();

            var inventoryItems = _dataHelper.GetInventoryItemFakerConfig().Generate(3);
            for (int i = 0; i < parts.Count(); i++)
            {
                inventoryItems[i].PartID = parts[i].ID;
                _testFixture.DbContext.InventoryItems.Add(inventoryItems[i]);
            }

            await _testFixture.DbContext.SaveChangesAsync();

            var result = await _testFixture.GetRequest<InventoryItemDetailListResponse>($"/api/inventory/index-detail?isCurrentOnly=true");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Should().BeEquivalentTo(inventoryItems[0]);
        }

        [Fact]
        public async Task Post_Should_CreateInventoryItemRecord()
        {
            var parts = await _dataHelper.AddPartsIfNeeded();
            var item = new InventoryItem { PartID = parts[0].ID, Quantity = 10, DateRecorded = DateTime.Today };

            var result = await _testFixture.PostRequest<InventoryItem, InventoryItemResponse>("/api/inventory", item);

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();

            var itemId = result?.Value?.ID;
            itemId.Should().BeGreaterThan(0);
            var savedItem = await _testFixture.DbContext.InventoryItems.FindAsync(itemId);
            
            savedItem.Should().NotBeNull();
            savedItem.Should().BeEquivalentTo(item, opt => opt.Excluding(i => i.ID));
        }

        [Fact]
        public async Task PostList_Should_CreateInventoryItemList()
        {
            var parts = await _dataHelper.AddPartsIfNeeded();
            var items = parts.Select(p => new InventoryItem { PartID = p.ID, Quantity = _dataHelper.GetRandomNumber(), DateRecorded = DateTime.Today }).ToList();
            
            var result = await _testFixture.PostRequest<List<InventoryItem>, InventoryItemListResponse>("/api/inventory/post-list", items);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();

            var savedItems = _testFixture.DbContext.InventoryItems;
            savedItems.Should().HaveCount(items.Count());
            savedItems.OrderBy(i => i.PartID).Should().BeEquivalentTo(items.OrderBy(t => t.PartID), opt => opt.Excluding(o => o.ID));
        }

        [Fact]
        public async Task Put_Should_UpdateInventoryItemRecord()
        {
            var savedItem = await _dataHelper.CreateInventoryItemInDatabase();
            savedItem?.ID.Should().BeGreaterThan(0);
            var itemModel = new InventoryItem { ID = savedItem.ID, PartID = savedItem.PartID.Value, Quantity = savedItem.Quantity + 10, DateRecorded = savedItem.DateRecorded.AddHours(2)};
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.PutRequest<InventoryItem, InventoryItemResponse>("/api/inventory", itemModel);

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();

            var itemId = result?.Value?.ID;
            Assert.True(itemId > 0);
            var updatedItem = await _testFixture.DbContext.InventoryItems.FindAsync(itemId);

            updatedItem.Should().NotBeNull();
            updatedItem.Should().BeEquivalentTo(itemModel, opt => opt.Excluding(p => p.ID));
        }

        [Fact]
        public async Task Delete_Should_DeleteInventoryItemRecord()
        {
            var savedItem = await _dataHelper.CreateInventoryItemInDatabase();
            savedItem?.ID.Should().BeGreaterThan(0);
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.DeleteRequest<InventoryItemResponse>($"/api/inventory/?id={savedItem.ID}");

            result.Value.Should().BeNull();
            result.HasError.Should().BeFalse();
            var deletedItem = await _testFixture.DbContext.InventoryItems.FindAsync(savedItem.ID);
            deletedItem.Should().BeNull();
        }
    }
}
