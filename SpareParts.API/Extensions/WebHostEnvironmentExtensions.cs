namespace SpareParts.API.Extensions
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsIntegrationTest(this IWebHostEnvironment environment)
        {
            return environment.IsEnvironment("IntegrationTest");
        }

        public static bool IsProduction(this IWebHostEnvironment environment)
        {
            return environment.IsEnvironment("Production");
        }
    }
}
