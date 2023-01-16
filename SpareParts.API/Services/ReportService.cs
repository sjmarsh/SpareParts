using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SpareParts.API.Services.PdfService;

namespace SpareParts.API.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateReport(ReportName reportName, object model);
    }

    public class ReportService : IReportService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IPdfService _pdfService;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IHttpContextAccessor httpContextAccessor, IRazorViewEngine razorViewEngine, IPdfService pdfService, ITempDataProvider tempDataProvider, ILogger<ReportService> logger)
        {
            Guard.Against.Null(httpContextAccessor);
            Guard.Against.Null(razorViewEngine);
            Guard.Against.Null(pdfService);
            Guard.Against.Null(tempDataProvider);
            Guard.Against.Null(logger);

            _httpContextAccessor = httpContextAccessor;
            _razorViewEngine = razorViewEngine;
            _pdfService = pdfService;
            _tempDataProvider = tempDataProvider;
            _logger = logger;
        }

        public async Task<byte[]> GenerateReport(ReportName reportName, object model)
        {
            var htmlString = await RenderToHtmlStringAsync($"/Reports/{reportName}.cshtml", model);
            return _pdfService.GeneratePdfFromHtmlString(reportName, htmlString);
        }

        private async Task<string> RenderToHtmlStringAsync(string razorViewName, object model)
        {
            if(_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
            {
                const string message = "HttpContext cannot be null when generating report.";
                _logger.LogError(message);
                throw new NullReferenceException(message);
            }

            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, new RouteData(), new ActionDescriptor());

            using var stringWriter = new StringWriter();
            var razorView = _razorViewEngine.GetView("", razorViewName, false);

            if (razorView == null || razorView.View == null)
            {
                _logger.LogError($"{razorViewName} not found. Unable to render report.");
                throw new ArgumentNullException(razorViewName);
            }

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };

            var viewContext = new ViewContext(
                actionContext,
                razorView.View,
                viewDataDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                stringWriter,
                new HtmlHelperOptions());

            await razorView.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }
}
