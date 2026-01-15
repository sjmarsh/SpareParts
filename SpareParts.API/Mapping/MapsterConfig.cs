using Mapster;

namespace SpareParts.API.Mapping
{
    public static class MapsterConfig 
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Entities.Part, Shared.Models.Part>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<Shared.Models.Part, Entities.Part>.NewConfig().PreserveReference(true);

            TypeAdapterConfig<Entities.PartAttribute, Shared.Models.PartAttribute>.NewConfig();
            TypeAdapterConfig<Shared.Models.PartAttribute, Entities.PartAttribute>.NewConfig();

            TypeAdapterConfig<Entities.Part, Shared.Models.PartGraphQLObject>.NewConfig().PreserveReference(true);
            TypeAdapterConfig<Shared.Models.PartGraphQLObject, Entities.Part>.NewConfig().PreserveReference(true);

            TypeAdapterConfig<Entities.InventoryItem, Shared.Models.InventoryItem>.NewConfig();
            TypeAdapterConfig<Shared.Models.InventoryItem, Entities.InventoryItem>.NewConfig();
            TypeAdapterConfig<Shared.Models.InventoryItemDetail, Entities.InventoryItem>.NewConfig();  // convert to standard item and drop the PartName
        }
    }
}
