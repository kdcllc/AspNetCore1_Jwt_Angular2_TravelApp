using Newtonsoft.Json;
using System.Collections.Generic;

namespace King.David.Consulting.Travel.Web.Domain
{
    public class City : BaseEntity
    {
        public int CityId { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        //Decimal(9,6) or ###.######

        [JsonIgnore]
        public State State { get; set; }
        public int StateId { get; set; }

        public List<UserVisit> UserVisits { get; set; }

    }
}

