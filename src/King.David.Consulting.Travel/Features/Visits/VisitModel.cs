using System;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class VisitModel
    {  
        public Guid VisitId { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        public DateTime VisitedOn { get; set; }

        public string UserName { get; set; }
    }
}
