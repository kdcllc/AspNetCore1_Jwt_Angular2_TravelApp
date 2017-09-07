using System.Collections.Generic;

namespace King.David.Consulting.Travel.Web.Features.Visits
{
    public class VisitsEnvelope
    {
        public VisitsEnvelope(List<VisitModel> visits, int pages)
        {
            Visits = visits;
            VisitsTotalCount = pages;
        }

        public List<VisitModel> Visits { get; set; }

        public int VisitsCount => Visits?.Count ?? 0;
        public int VisitsTotalCount { get; set; }
    }
}
