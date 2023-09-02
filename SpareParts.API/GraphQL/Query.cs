using AutoMapper;
using AutoMapper.QueryableExtensions;
using SpareParts.API.Data;
using SpareParts.API.Infrastructure;
using SpareParts.Shared.Models;

namespace SpareParts.API.GraphQL
{
    public class Query
    {
        private readonly IMapper _mapper;

        public Query(IMapper mapper)
        {
            _mapper = mapper;
        }

        [UseOffsetPaging(IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [AuthorizeByRoleHotChoc(Role.Administrator, Role.StocktakeUser)]
        public IQueryable<PartGraphQLObject> GetParts([Service] SparePartsDbContext dbContext)
        {
            return dbContext.Parts.ProjectTo<PartGraphQLObject>(_mapper.ConfigurationProvider);
        }
    }
}
