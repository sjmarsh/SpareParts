using Refit;
using Polly;

namespace SpareParts.Client
{
    public static class RefitExtensions
    {
        public static IHttpClientBuilder AddRefitClientFor<T>(this IServiceCollection services, string baseUri) where T : class
        {
            var settings = new RefitSettings(new NewtonsoftJsonContentSerializer());

            return services.AddRefitClient<T>(settings).ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUri);
            })
                .AddTransientHttpErrorPolicy(b => b.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5)
                }));
        }
    }
}
