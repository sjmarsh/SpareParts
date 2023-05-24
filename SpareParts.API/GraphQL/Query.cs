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

        [UseFiltering]
        [AuthorizeByRoleHotChoc(Role.Administrator, Role.StocktakeUser)]
        public IQueryable<Part> GetParts([Service] SparePartsDbContext dbContext)
        {
            return dbContext.Parts.ProjectTo<Part>(_mapper.ConfigurationProvider);
        }
    }
}
