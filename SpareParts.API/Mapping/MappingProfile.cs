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
        }
    }
}
