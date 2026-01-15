using FluentValidation;
using FluentValidation.Results;

namespace SpareParts.API.Services
{
    public interface IValidationHandler
    {
        Task<ValidationHandlerResult> ValidateAsync<T>(T instance);
    }  

    public class ValidationHandler : IValidationHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationHandler> _logger;

        public ValidationHandler(IServiceProvider serviceProvider, ILogger<ValidationHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<ValidationHandlerResult> ValidateAsync<T>(T instance)
        {
            if (_serviceProvider.GetService(typeof(IValidator<T>)) is IValidator<T> validator)
            {
                var validationResult = await validator.ValidateAsync(instance);
                return new ValidationHandlerResult(validationResult);
            }
            _logger.LogInformation(message: $"No validator found for type {typeof(T).Name}");
            return new ValidationHandlerResult();
        }   
    }

    public class ValidationHandlerResult
    {
        public ValidationHandlerResult()
        {
            IsValid = true;
            Errors = [];
        }

        public ValidationHandlerResult(ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            Errors = validationResult.Errors;

        }

        public bool IsValid { get; }
        public IList<ValidationFailure> Errors { get; }

        /// <summary>
        /// Gets a concatenated string containing all error messages, separated by semicolons.
        /// </summary>
        /// <remarks>This property is useful for displaying or logging multiple error messages in a
        /// single, human-readable format. If there are no errors, the returned string will be empty.</remarks>
        public string ErrorsString { get { return string.Join("; ", Errors.Select(e => e.ErrorMessage)); } }
    }
}
