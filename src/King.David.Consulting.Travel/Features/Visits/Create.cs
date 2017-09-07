using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class Create
    {
        public class VisitData
        {
            public string City { get; set; }

            public string State { get; set; }
        }

        public class VisitDataValidator : AbstractValidator<VisitData>
        {
            public VisitDataValidator()
            {
                RuleFor(x => x.City).NotNull().NotEmpty();
                RuleFor(x => x.State).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<Guid>
        {
            public VisitData Visit { get; set; }
            public string UserName { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Visit).NotNull().SetValidator(new VisitDataValidator());
                RuleFor(x => x.UserName).NotEmpty().NotNull();
            }
        }

        public class Handler : IAsyncRequestHandler<Command, Guid>
        {
            private AppDbContext _context;
            private ILogger<Handler> _logger;

            public Handler(AppDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }
            public async Task<Guid> Handle(Command message)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == message.UserName);
                    var city = await _context.Cities
                                        .Include(x => x.State)
                                        .FirstOrDefaultAsync(x => x.Name == message.Visit.City);
                    if (city == null)
                        throw new RestException(HttpStatusCode.NotFound);

                    var visit = new UserVisit()
                    {
                        CityId = city.CityId,
                        StateId = city.StateId,
                        UserId = user.UserId,
                        VisitedOn = DateTime.Now
                    };

                    _context.UserVisits.Add(visit);
                    var result = await _context.SaveChangesAsync();

                    return result == 0 ? Guid.Empty : visit.VisitId;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    throw new RestException(HttpStatusCode.BadRequest);
                }
               
            }
        }
    }
}
