using System.Collections.Generic;

namespace King.David.Consulting.Travel.Web.Features.Citities
{
    public class CitiesEnvelope
    {
        public CitiesEnvelope(List<CityModel> cities, int pages)
        {
            Cities = cities;
            CitiesTotalCount = pages;
        }
        public List<CityModel> Cities { get; set; }

        public int CitiesCount => Cities?.Count ?? 0;

        public int CitiesTotalCount { get; set; }
    }
}
