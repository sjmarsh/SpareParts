using System.Reflection;

namespace SpareParts.API.Infrastructure.SimpleMediator
{
    public interface ISimpleMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }

    public class SimpleMediator : ISimpleMediator
    {
        private readonly IServiceProvider _serviceProvider;
        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("Handle") ?? throw new InvalidOperationException("Handle method not found on handler.");
            var task = (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;
            return await task;
        }
    }

    public static class SimpleMediatorExtensions
    {
        public static IServiceCollection AddSimpleMediator(this IServiceCollection services, Assembly assembly)
        {
            services.AddTransient<ISimpleMediator, SimpleMediator>();
            var handlerInterfaceType = typeof(IRequestHandler<,>);
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                    .Select(i => new { HandlerType = t, InterfaceType = i }));
            foreach (var handler in handlerTypes)
            {
                services.AddTransient(handler.InterfaceType, handler.HandlerType);
            }
            return services;
        }
    }
}
