namespace SpareParts.API.Infrastructure
{
    public class ContentSecurityPolicyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;

        public ContentSecurityPolicyMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _hostEnvironment = hostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
            //Use "Content-Security-Policy-Report-Only" to fix new content that is being blocked - be aware that chrome has a false negative for wasmEval, so check if unable to resolve reported options
            const string frameworkBlazorWebassemblyJs = "sha256-v8v3RKRPmN4odZ1CWM5gw80QKPCCWMcpNeOmimNL2AA=";
            const string wasmEvalNotSupportedYet = "unsafe-eval";
            const string experimentalRecommendedCsp = "block-all-mixed-content; upgrade-insecure-requests; "; //see: https://docs.microsoft.com/en-us/aspnet/core/blazor/security/content-security-policy?view=aspnetcore-5.0#policy-directives
            string localWebSocket = _hostEnvironment.IsDevelopment() ? " ws: wss:" : "";  // used for hot-reloading.  Re-evaluate this setting if actually need web sockets in production

            context.Response.Headers.Add("Content-Security-Policy",
                "base-uri 'self'; "
                + "object-src 'none'; "
                + "default-src 'self'; "
                + $"script-src '{wasmEvalNotSupportedYet}' 'self'; "
                + $"script-src-elem 'self' '{frameworkBlazorWebassemblyJs}';"
                + "style-src 'self'; "
                + "style-src-elem 'self'; "
                + $"connect-src 'self'{localWebSocket}; "
                + "frame-src 'self'; "
                + "img-src 'self' data: https:; "
                + "frame-ancestors 'self'; "
                + "form-action 'none'; "
                + $"{experimentalRecommendedCsp}");


            await _next.Invoke(context);
        }
    }

    public static class ContentSecurityPolicyExtension
    {
        public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ContentSecurityPolicyMiddleware>();
        }
    }
}
