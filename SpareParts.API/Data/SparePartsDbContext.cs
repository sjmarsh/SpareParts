using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpareParts.API.Entities;

namespace SpareParts.API.Data
{
    public class SparePartsDbContext : IdentityDbContext<ApplicationUser>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        
        public DbSet<Part> Parts { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

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
                    if (property.ClrType.BaseType == typeof(Enum))
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
        }
    }
}
