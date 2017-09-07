using System.Linq;
using King.David.Consulting.Travel.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public static class VisitsExtention
    {
        public static IQueryable<UserVisit> GetAllData(this DbSet<UserVisit> visits)
        {
            return visits
                        .Include(x => x.State)
                        .Include(x => x.City)
                        .Include(x => x.User)
                        .AsNoTracking();
        }
    }
}
