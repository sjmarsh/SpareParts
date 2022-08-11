using Bogus;
using SpareParts.API.Data;

namespace SpareParts.Test.Helpers
{
    public class DataHelper
    {
        private readonly SparePartsDbContext _dbContext;
        const int NumberOfPartsForInventory = 10;

        public DataHelper(SparePartsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<API.Entities.Part> CreatePartInDatabase()
        {
            var partEntity = GetPartsFakerConfig().Generate(1).First();
            _dbContext.Parts.Add(partEntity);
            await _dbContext.SaveChangesAsync();
            return partEntity;
        }

        public async Task<List<API.Entities.Part>> CreatePartListInDatabase(int howMany)
        {
            var parts = GetPartsFakerConfig().Generate(howMany);
            foreach (var part in parts)
            {
                _dbContext.Parts.Add(part);
            }
            await _dbContext.SaveChangesAsync();

            return parts;
        }

        public Faker<API.Entities.Part> GetPartsFakerConfig()
        {
            return new Faker<API.Entities.Part>()
                        .RuleFor(p => p.ID, 0)
                        .RuleFor(p => p.Name, f => f.Name.JobTitle())
                        .RuleFor(p => p.Description, f => f.Name.JobDescriptor())
                        .RuleFor(p => p.Weight, f => f.Random.Number(0, 20))
                        .RuleFor(p => p.Price, f => f.Random.Number(0, 20))
                        .RuleFor(p => p.StartDate, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Today));
        }

        public async Task<API.Entities.InventoryItem> CreateInventoryItemInDatabase()
        {
            await AddPartsIfNeeded();
            var inventoryItemEntity = GetInventoryItemFakerConfig().Generate(1).First();
            _dbContext.InventoryItems.Add(inventoryItemEntity);
            await _dbContext.SaveChangesAsync();
            return inventoryItemEntity;
        }

        public async Task<List<API.Entities.InventoryItem>> CreateInventoryItemListInDatabase(int howMany)
        {
            await AddPartsIfNeeded();
            var items = GetInventoryItemFakerConfig().Generate(howMany);
            foreach (var item in items)
            {
                _dbContext.InventoryItems.Add(item);
            }
            await _dbContext.SaveChangesAsync();
            return items;
        }

        public async Task<List<Shared.Models.InventoryItemDetail>> CreateInventoryItemDetailListInDatabase(int howMany)
        {
            var parts = await AddPartsIfNeeded();
            var items = GetInventoryItemFakerConfig().Generate(howMany);
            foreach (var item in items)
            {
                _dbContext.InventoryItems.Add(item);
            }
            await _dbContext.SaveChangesAsync();

            var itemDetails = new List<Shared.Models.InventoryItemDetail>();
            foreach(var item in items)
            {
                itemDetails.Add(new()
                {
                    ID = item.ID,
                    PartID = item.PartID.Value,
                    PartName = parts.First(p => p.ID == item.PartID.Value).Name,
                    Quantity = item.Quantity,
                    DateRecorded = item.DateRecorded
                });
            }
            _dbContext.ChangeTracker.Clear();
            return itemDetails;
        }

        public Faker<API.Entities.InventoryItem> GetInventoryItemFakerConfig()
        {
            return new Faker<API.Entities.InventoryItem>()
                    .RuleFor(i => i.ID, 0)
                    .RuleFor(i => i.PartID, f => f.Random.Number(1, NumberOfPartsForInventory))
                    .RuleFor(i => i.Quantity, f => f.Random.Number(0, 100))
                    .RuleFor(i => i.DateRecorded, f => f.Date.Between(DateTime.Today.AddDays(-30), DateTime.Today));
        }

        public async Task<List<API.Entities.Part>> AddPartsIfNeeded()
        {
            // Inventory Items have a dependency on parts
            var parts = _dbContext.Parts;
            if (parts.Count() == 0)
            {
                return await CreatePartListInDatabase(NumberOfPartsForInventory);
            }
            return parts.ToList();
        }

        public int GetRandomNumber()
        {
            var random = new Random();
            return random.Next(1, 100);
        }

    }
}