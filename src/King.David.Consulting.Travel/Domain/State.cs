using Newtonsoft.Json;
using System.Collections.Generic;

namespace King.David.Consulting.Travel.Web.Domain
{
    public class State : BaseEntity
    {
        public int StateId { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        [JsonIgnore]
        public List<City> Cities { get; set; }

        [JsonIgnore]
        public List<UserVisit> UserVisits { get; set; }
    }

}
