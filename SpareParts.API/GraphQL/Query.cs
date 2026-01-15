using Mapster;
using SpareParts.API.Data;
using SpareParts.API.Infrastructure;
using SpareParts.Shared.Models;

namespace SpareParts.API.GraphQL
{
    public class Query
    {
        [UseOffsetPaging(IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        [AuthorizeByRoleHotChoc(Role.Administrator, Role.StocktakeUser)]
        public IQueryable<PartGraphQLObject> GetParts([Service] SparePartsDbContext dbContext)
        {
            return dbContext.Parts.ProjectToType<PartGraphQLObject>();
        }
    }
}
