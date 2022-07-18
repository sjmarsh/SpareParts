using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SpareParts.API.Data;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public interface IDataService
    {
        Task<TResponse> CreateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> UpdateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken)
           where TResponse : ResponseBase<TModel>, new()
           where TEntity : class
           where TModel : ModelBase;

        Task<TResponse> GetItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> GetList<TResponse, TEntity, TModel>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? filter = null) 
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;

        Task<TResponse> DeleteItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase;
    }

    public class DataService : IDataService
    {
        private readonly SparePartsDbContext _dbContext;
        private readonly IMapper _mapper;

        public DataService(SparePartsDbContext dbContext, IMapper mapper)
        {
            Guard.Against.Null(dbContext);
            Guard.Against.Null(mapper);

            _dbContext = dbContext;
            _mapper = mapper;
        }

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
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new TResponse { Value = _mapper.Map<TModel>(entity) };
        }

        public async Task<TResponse> UpdateItem<TResponse, TEntity, TModel>(TModel? model, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            if(model == null)
            {
                return new TResponse { HasError = true, Message = $"{typeof(TModel).Name} must be provided." };
            }

            var existingEntity = await _dbContext.Set<TEntity>().FindAsync(new object?[] { model.ID }, cancellationToken);
            if(existingEntity == null)
            {
                return new TResponse { HasError = true, Message = $"Unable to find existing {typeof(TModel).Name} record to be updated." };
            }
            _dbContext.Entry(existingEntity).State = EntityState.Modified;
            _mapper.Map(model, existingEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new TResponse { Value = model };
        }

        public async Task<TResponse> GetItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);
            if (entity == null)
            {
                return new TResponse { HasError = true, Message = $"Unable to find {typeof(TModel).Name} with ID {id}." };
            }
            else
            {
                var model = _mapper.Map<TModel>(entity);
                return new TResponse { Value = model };
            }
        }
        
        public async Task<TResponse> GetList<TResponse, TEntity, TModel>(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? filter = null)
            where TResponse : ResponseListBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            filter ??= ((e) => true);
            var items = await _dbContext.Set<TEntity>().Where(filter).ProjectTo<TModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return new TResponse { Items = items };
        }

        public async Task<TResponse> DeleteItem<TResponse, TEntity, TModel>(int id, CancellationToken cancellationToken)
            where TResponse : ResponseBase<TModel>, new()
            where TEntity : class
            where TModel : ModelBase
        {
            var existingEntity = await _dbContext.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
            if (existingEntity != null)
            {
                _dbContext.Remove(existingEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return new TResponse();
            }
            else
            {
                return new TResponse { HasError = true, Message = $"Unable to find existing {typeof(TModel).Name} record to delete." };
            }
        }
    }
}
