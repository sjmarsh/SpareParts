using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Services;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            Guard.Against.Null(mediator);
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<InventoryItemResponse> Get(int id) => await _mediator.Send(new GetInventoryItemRequest(id));

        [HttpGet]
        [Route("index")]
        public async Task<InventoryItemListResponse> Index() => await _mediator.Send(new GetInventoryItemListRequest());

        [HttpGet]
        [Route("index-detail")]
        public async Task<InventoryItemDetailListResponse> IndexDetail([FromQuery]GetInventoryItemDetailListRequest request) => await _mediator.Send(request);

        [HttpGet]
        [Route("report")]
        public async Task<IActionResult> Report(bool isCurrentOnly)
        {
            var report = await _mediator.Send(new CreateReportCommand { ReportName = ReportName.InventoryReport, IsCurrentOnly = isCurrentOnly });
            return new FileContentResult(report, "application/pdf");
        }

        [HttpPost]
        public async Task<InventoryItemResponse> Post(InventoryItem InventoryItem) => await _mediator.Send(new CreateInventoryItemCommand(InventoryItem));

        [HttpPost]
        [Route("post-list")]
        public async Task<InventoryItemListResponse> Post(List<InventoryItem> inventoryItems) => await _mediator.Send(new CreateInventoryItemListCommand(inventoryItems));

        [HttpPut]
        public async Task<InventoryItemResponse> Put(InventoryItem InventoryItem) => await _mediator.Send(new UpdateInventoryItemCommand(InventoryItem));

        [HttpDelete]
        public async Task<InventoryItemResponse> Delete(int id) => await _mediator.Send(new DeleteInventoryItemCommand(id));
    }
}
