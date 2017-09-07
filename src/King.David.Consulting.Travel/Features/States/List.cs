using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using King.David.Consulting.Common.AspNetCore.Errors;
using King.David.Consulting.Common.AspNetCore.Security;
using King.David.Consulting.Travel.Web.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.States
{
    public class List
    {
        #region Query with paging
        public class Query : IRequest<StatesEnvelope>
        {
            public Query(int? limit, int? offset)
            {
                Limit = limit;
                Offset = offset;
            }
            public int? Limit { get; set; }
            public int? Offset { get; set; }
            public string State { get; set; }
            public string UserName { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, StatesEnvelope>
        {
            private AppDbContext _context;

            public QueryHandler(AppDbContext context)
            {
                _context = context;
            }
            public async Task<StatesEnvelope> Handle(Query message)
            {
                IQueryable<State> queryable = _context.States.GetAllData();

                if (!string.IsNullOrWhiteSpace(message.UserName))
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x=> x.Username == message.UserName);
                    queryable = queryable.Where(x => x.UserVisits.Select(y => y.UserId).Contains(user.UserId));
                }

                var limit = message.Limit ?? 20;

                var totalPages = await queryable.CountAsync();

                if (queryable.Count() < limit)
                    limit = queryable.Count();

                var offset = message.Offset ?? 0;

                var states = await queryable.
                            OrderByDescending(x => x.DateAdded)
                            .Skip(offset)
                            .Take(limit)
                            .AsNoTracking()
                            .ToListAsync();

                return new StatesEnvelope(Mapper.Map<List<Domain.State>, List<StateModel>>(states),totalPages);
            }
        }
        #endregion

        //#region QueryByUserName

        //public class QueryByUserName : IRequest<StatesEnvelope>
        //{
        //    public QueryByUserName(string user)
        //    {
        //        Username = user;
        //    }
        //    public string Username { get; set; }
        //}
        //public class QueryByUserNameValidator : AbstractValidator<QueryByUserName>
        //{
        //    public QueryByUserNameValidator()
        //    {
        //        RuleFor(x => x.Username).NotNull().NotEmpty();
        //    }
        //}
        //public class QueryByUserNameHandler : IAsyncRequestHandler<QueryByUserName, StatesEnvelope>
        //{
        //    private AppDbContext _context;

        //    public QueryByUserNameHandler(AppDbContext context)
        //    {
        //        _context = context;
        //    }
        //    public async Task<StatesEnvelope> Handle(QueryByUserName message)
        //    {
        //        var user = await _context.Users
        //                     .AsNoTracking()
        //                     .FirstOrDefaultAsync(x => x.Username == message.Username);
        //        if (user == null)
        //            throw new RestException(HttpStatusCode.NotFound);
        //        IQueryable<State> queryable = _context.States.GetAllData();
        //        queryable = queryable.Where(x => x.UserVisits.Select(y => y.UserId).Contains(user.UserId));

        //        var result = await queryable.AsNoTracking()
        //                              .ToListAsync();
        //        #region
        //        //var query = await _context.UserVisits
        //        //            .Include(x => x.State)
        //        //            .Where(q => q.UserId == user.UserId)
        //        //            .Select(w=> w.State)
        //        //            .Distinct()
        //        //            .ToListAsync();
        //        #endregion
        //        return new StatesEnvelope(Mapper.Map<List<Domain.State>, List<StateModel>>(result));
        //    }
        //}
        //#endregion
        
    }
}
