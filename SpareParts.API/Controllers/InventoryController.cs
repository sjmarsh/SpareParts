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

        [HttpPost]
        public async Task<InventoryItemResponse> Post(InventoryItem InventoryItem) => await _mediator.Send(new CreateInventoryItemCommand(InventoryItem));

        [HttpPut]
        public async Task<InventoryItemResponse> Put(InventoryItem InventoryItem) => await _mediator.Send(new UpdateInventoryItemCommand(InventoryItem));

        [HttpDelete]
        public async Task<InventoryItemResponse> Delete(int InventoryItemId) => await _mediator.Send(new DeleteInventoryItemCommand(InventoryItemId));
    }
}
