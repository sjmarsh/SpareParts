using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Services;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PartController(IMediator mediator)
        {
            Guard.Against.Null(mediator);
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<PartResponse> Get(int id) => await _mediator.Send(new GetPartRequest(id));  
        
        [HttpGet]
        [Route("index")]
        public async Task<PartListResponse> Index() => await _mediator.Send(new GetPartListRequest());
        
        [HttpPost]
        public async Task<PartResponse> Post(Part part) => await _mediator.Send(new CreatePartCommand(part));
        
        [HttpPut]    
        public async Task<PartResponse> Put(Part part) => await _mediator.Send(new UpdatePartCommand(part));
        
        [HttpDelete]
        public async Task<PartResponse> Delete(int partId) => await _mediator.Send(new DeletePartCommand(partId));
    }
}
