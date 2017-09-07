using System;

namespace King.David.Consulting.Travel.Web.Domain
{
    public class UserVisit
    {
        public UserVisit()
        {
            this.VisitId = Guid.NewGuid();
        }
        public Guid VisitId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public int StateId { get; set; }

        public State State { get; set; }

        public DateTime VisitedOn { get; set; }

    }
}
