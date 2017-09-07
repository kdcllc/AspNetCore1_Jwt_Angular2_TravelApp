using System;
using System.Threading.Tasks;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Common.AspNetCore.Security.Token;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class VisitsController : Controller
    {
        private IMediator _mediator;
        private ICurrentUserAccessor _currentUserAccessor;

        public VisitsController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        #region Required Get & Create & Delete Implementation
        [HttpGet("user/{user}/visits")]
        public async Task<VisitsEnvelope> GetUserVisitsRequired(string user, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset) { UserName = user });
        }

        [HttpPost("user/{user}/visits")]
        public async Task<IActionResult> CreateVisitRequired([FromBody]Create.VisitData visit, string user)
        {
            return await Create(visit, user);
        }

        [HttpDelete("user/{user}/visit/{visit}")]
        public async Task DeleteVisitRequired(string user, Guid visit)
        {
            await _mediator.Send(new Delete.Command(visit));
        }
        #endregion

        [HttpGet("visits")]
        public async Task<VisitsEnvelope> Get([FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset));
        }

        [HttpPost("visits")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task<IActionResult> Post([FromBody]Create.VisitData visit)
        {
              return await Create(visit, _currentUserAccessor.GetCurrentUsername());
        }

        [HttpDelete("visits/{visit}")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task Delete(Guid visit)
        {
            if (_currentUserAccessor.GetCurrentUsername() != null)
                await _mediator.Send(new Delete.Command(visit));
        }
    
        [HttpGet("visits/user/{user}")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task<VisitsEnvelope> GetByAuthUser(string user, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            return await _mediator.Send(new List.Query(limit, offset){UserName = user });
        }

        private async Task<IActionResult> Create(Create.VisitData visit, string user)
        {
            var result = await _mediator.Send(new Create.Command { Visit = visit, UserName = user });

            if (result != Guid.Empty)
                return Created("", result);
            else
                return BadRequest();
        }
    }
}
