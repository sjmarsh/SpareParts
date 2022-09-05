using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Services;
using SpareParts.API.Services.Reports;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPartReportService _partReportService;

        public PartController(IMediator mediator, IPartReportService partReportService)
        {
            Guard.Against.Null(mediator);
            _mediator = mediator;
            _partReportService = partReportService;
        }

        [HttpGet]
        public async Task<PartResponse> Get(int id) => await _mediator.Send(new GetPartRequest(id));  
        
        [HttpGet]
        [Route("index")]
        public async Task<PartListResponse> Index([FromQuery]GetPartListRequest request) => await _mediator.Send(request);

        [HttpGet]
        [Route("report")]
        public async Task<string> Report()
        {
            return await _partReportService.GetPartListReport(HttpContext, cancellationToken: default);
        }

        [HttpPost]
        public async Task<PartResponse> Post(Part part) => await _mediator.Send(new CreatePartCommand(part));
        
        [HttpPut]    
        public async Task<PartResponse> Put(Part part) => await _mediator.Send(new UpdatePartCommand(part));
        
        [HttpDelete]
        public async Task<PartResponse> Delete(int id) => await _mediator.Send(new DeletePartCommand(id));
    }
}
