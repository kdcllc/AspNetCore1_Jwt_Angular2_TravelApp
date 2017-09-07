using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class List
    {
        public class Query : IRequest<VisitsEnvelope>
        {
            public Query(int? limit, int? offset)
            {
                Limit = limit;
                Offset = offset;
            }

            public string UserName { get; set; }
            public int? Limit { get; }
            public int? Offset { get; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, VisitsEnvelope>
        {
            private AppDbContext _context;
            private ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(AppDbContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }
            public async Task<VisitsEnvelope> Handle(Query message)
            {
                IQueryable<UserVisit> queryable = _context.UserVisits.GetAllData();

                if (!string.IsNullOrWhiteSpace(message.UserName))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == message.UserName);
                    queryable = queryable.Where(x => x.UserId == user.UserId);
                }

                var limit = message.Limit ?? 20;

                var totalPages = await queryable.CountAsync();

                if (totalPages < limit)
                    limit = queryable.Count();

                var offset = message.Offset ?? 0;

                var query = await queryable
                                       .OrderBy(x => x.VisitedOn)
                                       .Skip(offset)
                                       .Take(limit)
                                       .AsNoTracking()
                                       .ToListAsync();

                List<VisitModel> visits = new List<VisitModel>();

                foreach (var item in query)
                {
                    var visit = new VisitModel
                    {
                        VisitId = item.VisitId,
                        UserName = item.User.Username,
                        City = item.City.Name,
                        State = item.State.Abbreviation,
                        VisitedOn = item.VisitedOn
                    };

                    visits.Add(visit);
                }

                return new VisitsEnvelope(visits, totalPages);
            }
        }
    }
}
