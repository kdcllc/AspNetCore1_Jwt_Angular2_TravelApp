using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace King.David.Consulting.Travel.Web.Features.Citities
{
    public class CitiesController : Controller
    {
        private IMediator _mediator;

        public CitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("cities")]
        public async Task<CitiesEnvelope> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit,offset));
        }

        [HttpGet("cities/{user}")]
        public async Task<CitiesEnvelope> Get(string user, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset) {UserName = user});
        }

        [HttpGet("state/{state}/cities")]
        public async Task<CitiesEnvelope> GetCitiesByStateRequired(string state, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset) { State = state});
        }

    }
}
