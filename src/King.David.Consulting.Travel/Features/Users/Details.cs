using AutoMapper;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace King.David.Consulting.Travel.Web.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Username { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, UserEnvelope>
        {
            private readonly AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<UserEnvelope> Handle(Query message)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                return new UserEnvelope(Mapper.Map<Domain.User, User>(user));
            }
        }
    }
}
