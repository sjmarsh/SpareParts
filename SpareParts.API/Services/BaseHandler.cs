using Ardalis.GuardClauses;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public class BaseHandler
    {
        protected readonly IDataService _dataService;
        protected readonly ILogger _logger;

        public BaseHandler(IDataService dataService, ILogger logger)
        {
            Guard.Against.Null(dataService);
            Guard.Against.Null(logger);

            _dataService = dataService;
            _logger = logger;
        }

        protected TResponse ReturnAndLogException<TResponse, TModel>(string message, Exception ex) where TResponse : ResponseBase<TModel>, new() where TModel : ModelBase
        {
            _logger.LogError(message, ex);
            return new TResponse { HasError = true, Message = message };
        }

        protected TResponse ReturnListAndLogException<TResponse, TModel>(string message, Exception ex) where TResponse : ResponseListBase<TModel>, new() where TModel : ModelBase
        {
            _logger.LogError(message, ex);
            return new TResponse { HasError = true, Message = message };
        }
    }
}
