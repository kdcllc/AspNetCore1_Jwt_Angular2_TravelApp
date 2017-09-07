using System.Linq;
using King.David.Consulting.Travel.Web.Domain;
using Microsoft.EntityFrameworkCore;

namespace King.David.Consulting.Travel.Web.Features.Citities
{
    public static class CityExtentions
    {
        public static IQueryable<City> GetAllData(this DbSet<City> cities)
        {
            return cities
                .Include(x => x.State)
                .AsNoTracking();
        }
    }
}
