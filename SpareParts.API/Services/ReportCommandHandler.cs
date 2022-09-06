using Ardalis.GuardClauses;
using Humanizer;
using MediatR;
using OpenHtmlToPdf;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public enum ReportName
    {
        PartsListReport
    }

    public class CreateReportCommand : IRequest<byte[]>
    {
        public ReportName ReportName { get; set; }
    }

    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, byte[]>
    {
        private readonly IReportService _reportService;
        private readonly IDataService _dataService;
        
        public CreateReportCommandHandler(IReportService reportService, IDataService dataService)
        {
            Guard.Against.Null(reportService);
            Guard.Against.Null(dataService);
            
            _reportService = reportService;
            _dataService = dataService;            
        }

        public async Task<byte[]> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var model = await GetReportModelFor(request.ReportName, cancellationToken);
            var reportHtmlString = await _reportService.RenderToHtmlStringAsync($"/Reports/{request.ReportName}.cshtml", model);
            
            // TODO - incorporate this into report service
            // ref: https://github.com/vilppu/OpenHtmlToPdf
            var pdf = Pdf.From(reportHtmlString)
                .WithTitle(request.ReportName.ToString().Humanize(LetterCasing.Title))
                .OfSize(PaperSize.A4)
                .Portrait()
                .WithObjectSetting("web.userStyleSheet", "./Reports/reports.css")
                .Comressed()
                .Content();

            return pdf;
        }

        private async Task<object> GetReportModelFor(ReportName reportName, CancellationToken cancellationToken)
        {
            object? model;
            switch (reportName)
            {
                case ReportName.PartsListReport:
                    model = await _dataService.GetList<PartListResponse, Entities.Part, Part>(cancellationToken);
                    break;
                default:
                    throw new NotSupportedException($"{reportName} is not supported. Register a model for this report.");
            }

            return model;
        }
    }
}
