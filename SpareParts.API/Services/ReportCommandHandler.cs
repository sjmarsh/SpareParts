using Ardalis.GuardClauses;
using MediatR;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
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
            var model = await GetReportModel(request.ReportName, cancellationToken);
            return await _reportService.GenerateReport(request.ReportName, model);
        }

        private async Task<object> GetReportModel(ReportName reportName, CancellationToken cancellationToken)
        {
            object? model;
            switch (reportName)
            {
                case ReportName.PartListReport:
                    model = await _dataService.GetList<PartListResponse, Entities.Part, Part>(cancellationToken);
                    break;
                default:
                    throw new NotSupportedException($"{reportName} is not supported. Register a model for this report.");
            }

            return model;
        }
    }
}
