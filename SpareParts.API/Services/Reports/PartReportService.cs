using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services.Reports
{
    public interface IPartReportService
    {
        Task<string> GetPartListReport(HttpContext httpContext, CancellationToken cancellationToken);
    }

    public class PartReportService : IPartReportService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IDataService _dataService;
        private readonly ILogger<PartReportService> _logger;

        public PartReportService(IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider, IDataService dataService, ILogger<PartReportService> logger)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _dataService = dataService;
            _logger = logger;
        }

        public async Task<string> GetPartListReport(HttpContext httpContext, CancellationToken cancellationToken)
        {
            var parts = await _dataService.GetList<PartListResponse, Entities.Part, Part>(cancellationToken);
            var partsReport = await RenderToStringAsync(httpContext, "/Services/Reports/PartsListReport.cshtml", parts);
            return partsReport;
        }

        public async Task<string> RenderToStringAsync(HttpContext httpContext, string viewName, object model)
        {
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using var stringWriter = new StringWriter();
            var razorView = _razorViewEngine.GetView("", viewName, false);

            if (razorView == null || razorView.View == null)
            {
                _logger.LogError($"{viewName} not found. Unable to render report.");
                throw new ArgumentNullException(viewName);
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
