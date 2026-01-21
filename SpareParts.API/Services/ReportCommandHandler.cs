using Ardalis.GuardClauses;
using SpareParts.API.Infrastructure.SimpleMediator;

namespace SpareParts.API.Services
{
    public class CreateReportCommand : IRequest<byte[]>
    {
        public ReportName ReportName { get; set; }
        public bool IsCurrentOnly { get; set; }
    }

    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, byte[]>
    {
        private readonly IReportService _reportService;
        private readonly IDataService _dataService;
        private readonly ISimpleMediator _mediator;

        public CreateReportCommandHandler(IReportService reportService, IDataService dataService, ISimpleMediator mediator)
        {
            Guard.Against.Null(reportService);
            Guard.Against.Null(dataService);
            Guard.Against.Null(mediator);
            
            _reportService = reportService;
            _dataService = dataService;
            _mediator = mediator;
        }

        public async Task<byte[]> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var model = await GetReportModel(request.ReportName, request.IsCurrentOnly, cancellationToken);
            return await _reportService.GenerateReport(request.ReportName, model);
        }

        private async Task<object> GetReportModel(ReportName reportName, bool isCurrentOnly, CancellationToken cancellationToken)
        {
            object? model = reportName switch
            {
                ReportName.PartListReport => await _mediator.Send(new GetPartListRequest()),
                ReportName.InventoryReport => await _mediator.Send(new GetInventoryItemDetailListRequest { IsCurrentOnly = isCurrentOnly }),
                _ => throw new NotSupportedException($"{reportName} is not supported. Register a model for this report."),
            };
            return model;
        }
    }
}
