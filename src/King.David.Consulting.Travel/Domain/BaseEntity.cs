using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace King.David.Consulting.Travel.Web.Domain
{
    public class BaseEntity
    {
        public DateTime DateAdded { get; set; }
        public DateTime DateTimeAdded { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
