using AutoMapper;

namespace SpareParts.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Part, Shared.Models.Part>();
            CreateMap<Shared.Models.Part, Entities.Part>().ReverseMap();

            CreateMap<Entities.InventoryItem, Shared.Models.InventoryItem>();
            CreateMap<Shared.Models.InventoryItem, Entities.InventoryItem>().ReverseMap();
            CreateMap<Shared.Models.InventoryItemDetail, Entities.InventoryItem>().ReverseMap();  // convert to standard item and drop the PartName
        }
    }
}
