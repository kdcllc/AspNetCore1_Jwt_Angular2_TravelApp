using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace King.David.Consulting.Travel.Web.Features.States
{

    public class StatesController : Controller
    {
        private IMediator _mediator;

        public StatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("states")]
        public async Task<StatesEnvelope> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit,offset));
        }

        [HttpGet("user/{user}/visits/states")]
        public async Task<StatesEnvelope> GetStatesByUser(string user, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset) { UserName = user});
        }
    }
}
