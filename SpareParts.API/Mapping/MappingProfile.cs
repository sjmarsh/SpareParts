using AutoMapper;

namespace SpareParts.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Part, Shared.Models.Part>();
            CreateMap<Shared.Models.Part, Entities.Part>().ReverseMap();

            CreateMap<Entities.PartAttribute, Shared.Models.PartAttribute>();
            CreateMap<Shared.Models.PartAttribute, Entities.PartAttribute>().ReverseMap();

            CreateMap<Entities.Part, Shared.Models.PartGraphQLObject>();
            CreateMap<Shared.Models.PartGraphQLObject, Entities.Part>().ReverseMap();

            CreateMap<Entities.InventoryItem, Shared.Models.InventoryItem>();
            CreateMap<Shared.Models.InventoryItem, Entities.InventoryItem>().ReverseMap();
            CreateMap<Shared.Models.InventoryItemDetail, Entities.InventoryItem>().ReverseMap();  // convert to standard item and drop the PartName
        }
    }
}
