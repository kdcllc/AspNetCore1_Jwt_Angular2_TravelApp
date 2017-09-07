using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Command(Guid visitId)
            {
                VisitId = visitId;
            }

            public Guid VisitId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.VisitId).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Command>
        {
            private AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task Handle(Command message)
            {
                var visit = _context.UserVisits.FirstOrDefault(x => x.VisitId == message.VisitId);

                if (visit == null)
                    throw new RestException(HttpStatusCode.NotFound);

                _context.UserVisits.Remove(visit);
                await _context.SaveChangesAsync();
            }
        }
    }
}
