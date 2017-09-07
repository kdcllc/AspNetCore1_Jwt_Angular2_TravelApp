using System.Collections.Generic;

namespace King.David.Consulting.Travel.Web.Features.States
{
    public class StatesEnvelope
    {
        public StatesEnvelope(List<StateModel> states, int pages)
        {
            States = states;
            StatesTotalCount = pages;
        }
        public List<StateModel> States { get; set; }

        public int StatesCount => States?.Count ?? 0;

        public int StatesTotalCount { get; set; }
    }
}
