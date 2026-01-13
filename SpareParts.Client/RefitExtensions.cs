using Refit;
using Polly;
using SpareParts.Client.Services.Authentication;

namespace SpareParts.Client
{
    public static class RefitExtensions
    {
        public static IHttpClientBuilder AddRefitClientFor<T>(this IServiceCollection services, string baseUri) where T : class
        {
            return services.AddRefitClient<T>(new RefitSettings()).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUri);
            })
                .AddHttpMessageHandler<AuthHeaderHandler>()
                .AddTransientHttpErrorPolicy(b => b.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5)
                }));
        }
    }
}
