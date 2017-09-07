using System.Linq;
using King.David.Consulting.Travel.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.States
{
    public static class StateExtensions
    {
        public static IQueryable<State> GetAllData(this DbSet<State> states)
        {
            return states
                .Include(x => x.Cities)
                .AsNoTracking();
        }
    }
}
