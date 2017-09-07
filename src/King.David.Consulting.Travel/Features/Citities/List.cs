using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.Citities
{
    public class List
    {
        #region Query with paging
        public class Query : IRequest<CitiesEnvelope>
        {
            public Query(int? limit, int? offset)
            {
                Limit = limit;
                Offset = offset;
            }
            public int? Limit { get; }
            public int? Offset { get; }

            public string UserName { get; set; }

            public string State { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, CitiesEnvelope>
        {
            private AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }
            public async Task<CitiesEnvelope> Handle(Query message)
            {
                IQueryable<City> queryable = _context.Cities.GetAllData();

                //user logged in
                if (!string.IsNullOrWhiteSpace(message.UserName))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == message.UserName);
                    if (user == null)
                        throw new RestException(HttpStatusCode.NotFound);
                    queryable = queryable.
                                Where(x => x.UserVisits.Any(y => y.UserId == user.UserId));
                }

                if (!string.IsNullOrWhiteSpace(message.State))
                {
                    queryable = queryable
                                        .Where(x => x.State.Abbreviation.Contains(message.State.ToUpper()));
                }
                var limit = message.Limit ?? 20;
                var totalPages = await queryable.CountAsync();

                if (queryable.Count() < limit)
                    limit = queryable.Count();

                var offset = message.Offset ?? 0;

                var cities = await queryable
                                       .OrderBy(x => x.Name)
                                       .Skip(offset)
                                       .Take(limit)
                                       .AsNoTracking()
                                       .ToListAsync();

                return new CitiesEnvelope(Mapper.Map<List<Domain.City>, List<CityModel>>(cities),totalPages);
            }
        }

        #endregion
 
    }
}
