using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SpareParts.API.Data;
using SpareParts.Shared.Models;
using SpareParts.API.Entities;

namespace SpareParts.API.Services
{
    public interface IDataService
    {
        Task<TResponse> CreateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> CreateList<TResponse, TEntity, TModel>(List<TModel>? modelList, CancellationToken cancellationToken)
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> UpdateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken, string[]? referencedCollectionsToUpdate = null)
           where TResponse : ResponseBase<TModel>, new()
           where TEntity : EntityBase
           where TModel : ModelBase;

        Task<TResponse> GetItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken, string[]? referencedCollections = null)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> GetList<TResponse, TEntity, TModel>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? filter = null, int skip = 0, int? take = null) 
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> DeleteItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        bool HasRelatedItems<T>(Expression<Func<T, bool>> matchPredicate) where T : class;
    }

    public class DataService : IDataService
    {
        private readonly IMapper _mapper;

        public DataService(SparePartsDbContext dbContext, IMapper mapper)
        {
            Guard.Against.Null(dbContext);
            Guard.Against.Null(mapper);

            DbContext = dbContext;
            _mapper = mapper;
        }

        public SparePartsDbContext DbContext { get; private set; }

        public async Task<TResponse> CreateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            if(model == null)
            {
                return new TResponse { HasError = true, Message = $"{typeof(TModel).Name} must be provided." };
            }

            var entity = _mapper.Map<TEntity>(model);
            DbContext.Set<TEntity>().Add(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
            return new TResponse { Value = _mapper.Map<TModel>(entity) };
        }

        public async Task<TResponse> CreateList<TResponse, TEntity, TModel>(List<TModel>? modelList, CancellationToken cancellationToken)
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            if (modelList == null)
            {
                return new TResponse { HasError = true, Message = $"{typeof(TModel).Name} list must be provided." };
            }
            var entities = modelList.AsQueryable().ProjectTo<TEntity>(_mapper.ConfigurationProvider);
            foreach(var entity in entities)
            {
                DbContext.Set<TEntity>().Add(entity);
            }
            await DbContext.SaveChangesAsync(cancellationToken);
            var items = entities.ProjectTo<TModel>(_mapper.ConfigurationProvider).ToList();
            return new TResponse { Items = items };
        }

        public async Task<TResponse> UpdateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken, string[]? referencedCollectionsToUpdate = null)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : EntityBase
            where TModel : ModelBase
        {
            if(model == null)
            {
                return new TResponse { HasError = true, Message = $"{typeof(TModel).Name} must be provided." };
            }

            var existingEntity = await DbContext.Set<TEntity>().FindAsync(new object?[] { model.ID }, cancellationToken);
            if(existingEntity == null)
            {
                return new TResponse { HasError = true, Message = $"Unable to find existing {typeof(TModel).Name} record to be updated." };
            }
            var dbEntry = DbContext.Entry(existingEntity);
            dbEntry.State = EntityState.Modified;

            _mapper.Map(model, existingEntity);
            

            if (referencedCollectionsToUpdate != null) 
            {
                foreach (var referencedCollection in referencedCollectionsToUpdate)
                {
                    var dbCollection = dbEntry.Collection(referencedCollection);
                    var dbCollectionAccessor = dbCollection.Metadata.GetCollectionAccessor();

                    await dbCollection.LoadAsync();

                    if (dbCollection != null && dbCollection.CurrentValue != null && dbCollectionAccessor != null)
                    {
                        var dbItemsMap = ((IEnumerable<EntityBase>)dbCollection.CurrentValue).ToDictionary(e => e.ID);
                        var items = (IEnumerable<EntityBase>)dbCollectionAccessor.GetOrCreate(existingEntity, false);

                        foreach (var item in items)
                        {
                            if (!dbItemsMap.TryGetValue(item.ID, out var oldItem)) // doesn't exist
                            {
                                dbCollectionAccessor.Add(existingEntity, item, false);  // add it
                            }
                            else
                            {
                                DbContext.Entry(oldItem).CurrentValues.SetValues(item);  // update
                                dbItemsMap.Remove(item.ID);
                            }
                        }

                        foreach (var oldItem in dbItemsMap.Values)
                        {
                            dbCollectionAccessor.Remove(existingEntity, oldItem);
                        }
                    }
                }
            }

            

            await DbContext.SaveChangesAsync(cancellationToken);
            return new TResponse { Value = model };
        }

        public async Task<TResponse> GetItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken, string[]? referencedCollections = null)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);
            if (entity == null)
            {
                return new TResponse { HasError = true, Message = $"Unable to find {typeof(TModel).Name} with ID {id}." };
            }
            else
            {
                if(referencedCollections != null)
                {
                    foreach(var reference in referencedCollections)
                    {
                        await DbContext.Entry(entity).Collection(reference).LoadAsync();
                    }
                }
                var model = _mapper.Map<TModel>(entity);
                return new TResponse { Value = model };
            }
        }
        
        public async Task<TResponse> GetList<TResponse, TEntity, TModel>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? filter = null, int skip = 0, int? take = null)
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            filter ??= ((e) => true);
            var qry = DbContext.Set<TEntity>().Where(filter);
            var totalCount = qry.Count();
            take ??= totalCount;
            var items = await qry.Skip(skip).Take(take.Value).ProjectTo<TModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return new TResponse { Items = items, TotalItemCount =  totalCount };
        }

        public async Task<TResponse> DeleteItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            var existingEntity = await DbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
            if (existingEntity != null)
            {
                DbContext.Remove(existingEntity);
                await DbContext.SaveChangesAsync(cancellationToken);
                return new TResponse();
            }
            else
            {
                return new TResponse { HasError = true, Message = $"Unable to find existing {typeof(TModel).Name} record to delete." };
            }
        }

        public bool HasRelatedItems<T>(Expression<Func<T, bool>> matchPredicate) where T : class
        {
            return DbContext.Set<T>().Any(matchPredicate);
        }
    }
}
