using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpareParts.API.Entities;

#nullable disable

namespace SpareParts.API.Data
{
    public class SparePartsDbContext : IdentityDbContext<ApplicationUser>
    {        
        public DbSet<Part> Parts { get; set; }

        public DbSet<PartAttribute> PartAttribute { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public SparePartsDbContext(DbContextOptions<SparePartsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            foreach (IMutableEntityType entityType in entityTypes)
            {
                //This will singularize all table names
                entityType.SetTableName(entityType.GetTableName());

                var props = entityType.GetProperties().ToList();
                foreach (var property in props)
                {
                    // Store Enums as Strings
                    if (property.ClrType.BaseType == typeof(Enum)) // warning: this doesn't detect nullable enums!
                    {
                        var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                        var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

                        property.SetValueConverter(converter);
                    }
                }

                base.OnModelCreating(modelBuilder);
            }

            modelBuilder.Entity<Part>()
                .HasMany<InventoryItem>()
                .WithOne()
                .HasForeignKey(i => i.PartID);

            modelBuilder.Entity<Part>().Property(p => p.Category)
                .HasConversion(c => c.ToString(), c => GetPartCategory(c));

            modelBuilder.Entity<PartAttribute>()
                .HasOne<Part>()
                .WithMany(p => p.Attributes)
                .OnDelete(DeleteBehavior.ClientCascade);

            SeedData(modelBuilder);
        }

        private static Shared.Models.PartCategory? GetPartCategory(string c)
        {
            if (string.IsNullOrWhiteSpace(c))
            {
                return null;
            }

            return (Shared.Models.PartCategory)Enum.Parse(typeof(Shared.Models.PartCategory), c);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // NOTE: Updated to use fixed dates after updating to EF ver 9.  Errors were being raised around data inconsistencies when using new DateTime() to seed data.  This would incorrectly raise a model change detection and fail to run.
            // More details here:  https://github.com/dotnet/efcore/issues/35158
            // and:  https://github.com/dotnet/efcore/issues/35158#issuecomment-2504830752
            modelBuilder.Entity<Part>().HasData(
                new[]
                {
                    new Part { ID = 1, Name = "Part 1", Description = "The first part", Price = 10.1, Weight = 1.11, StartDate = new DateTime(2011, 01, 01), Category = Shared.Models.PartCategory.Electronic },
                    new Part { ID = 2, Name = "Part 2", Description = "The second part", Price = 12.22, Weight = 2.22,StartDate = new DateTime(2012, 02, 02), Category = Shared.Models.PartCategory.Miscellaneous },
                    new Part { ID = 3, Name = "Part 3", Description = "The third part", Price = 13.3, Weight = 3.33,StartDate = new DateTime(2013, 03, 03), EndDate = new DateTime(2033, 03, 03) },
                    new Part { ID = 4, Name = "Part 4", Description = "The fourth part", Price = 14.4, Weight = 4.44, StartDate = new DateTime(2014, 04, 04), EndDate = new DateTime(2034, 04, 04) }
                });

            modelBuilder.Entity<InventoryItem>().HasData(
                new[] 
                {
                    new InventoryItem { ID = 1, PartID = 1, Quantity = 11, DateRecorded = new DateTime(2011, 01, 01) },
                    new InventoryItem { ID = 2, PartID = 1, Quantity = 13, DateRecorded = new DateTime(2012, 02, 02) },
                    new InventoryItem { ID = 3, PartID = 1, Quantity = 5, DateRecorded = new DateTime(2013, 03, 03) },
                    new InventoryItem { ID = 4, PartID = 2, Quantity = 22, DateRecorded = new DateTime(2012, 02, 22) },
                    new InventoryItem { ID = 5, PartID = 2, Quantity = 16, DateRecorded = new DateTime(2012, 02, 23) },
                    new InventoryItem { ID = 6, PartID = 2, Quantity = 1, DateRecorded = new DateTime(2013, 02, 03) },
                    new InventoryItem { ID = 7, PartID = 3, Quantity = 33, DateRecorded = new DateTime(2013, 03, 03) },
                    new InventoryItem { ID = 8, PartID = 3, Quantity = 50, DateRecorded = new DateTime(2013, 03, 04) },
                    new InventoryItem { ID = 9, PartID = 3, Quantity = 40, DateRecorded = new DateTime(2013, 03, 05) }
                }); 
        }
    }
}
