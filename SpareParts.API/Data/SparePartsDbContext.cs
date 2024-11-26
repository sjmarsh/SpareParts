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
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                //This will singularize all table names
                entityType.SetTableName(entityType.GetTableName());

                foreach (var property in entityType.GetProperties())
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


        // This was added as a work-around to a breaking change when updating to ef version 9.0
        // ref: https://github.com/dotnet/efcore/issues/35158
        // TODO - remove this when issue resolved
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));

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
            modelBuilder.Entity<Part>().HasData(
                new[]
                {
                    new Part { ID = 1, Name = "Part 1", Description = "The first part", Price = 10.1, Weight = 1.11, StartDate = DateTime.Now.AddMonths(-6), Category = Shared.Models.PartCategory.Electronic },
                    new Part { ID = 2, Name = "Part 2", Description = "The second part", Price = 12.22, Weight = 2.22,StartDate = DateTime.Now.AddMonths(-5), Category = Shared.Models.PartCategory.Miscellaneous },
                    new Part { ID = 3, Name = "Part 3", Description = "The third part", Price = 13.3, Weight = 3.33,StartDate = DateTime.Now.AddMonths(-4), EndDate = DateTime.Now.AddMonths(6) },
                    new Part { ID = 4, Name = "Part 4", Description = "The fourth part", Price = 14.4, Weight = 4.44, StartDate = DateTime.Now.AddYears(-1), EndDate = DateTime.Now.AddMonths(-1) }
                });

            modelBuilder.Entity<InventoryItem>().HasData(
                new[] 
                {
                    new InventoryItem { ID = 1, PartID = 1, Quantity = 11, DateRecorded = DateTime.Now.AddMonths(-6) },
                    new InventoryItem { ID = 2, PartID = 1, Quantity = 13, DateRecorded = DateTime.Now.AddMonths(-5) },
                    new InventoryItem { ID = 3, PartID = 1, Quantity = 5, DateRecorded = DateTime.Now.AddMonths(-4) },
                    new InventoryItem { ID = 4, PartID = 2, Quantity = 22, DateRecorded = DateTime.Now.AddMonths(-5) },
                    new InventoryItem { ID = 5, PartID = 2, Quantity = 16, DateRecorded = DateTime.Now.AddMonths(-4) },
                    new InventoryItem { ID = 6, PartID = 2, Quantity = 1, DateRecorded = DateTime.Now.AddMonths(-3) },
                    new InventoryItem { ID = 7, PartID = 3, Quantity = 33, DateRecorded = DateTime.Now.AddMonths(-4) },
                    new InventoryItem { ID = 8, PartID = 3, Quantity = 50, DateRecorded = DateTime.Now.AddMonths(-3) },
                    new InventoryItem { ID = 9, PartID = 3, Quantity = 40, DateRecorded = DateTime.Now.AddMonths(-2) }
                }); 
        }
    }
}
