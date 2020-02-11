using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Models
{
    public class YourTlevelsViewModel
    {
        public int RouteId { get; set; }
        public int PathId { get; set; }
        public string TLevelDescription { get; set; }
        public string TLevelStatus { get; set; }
    }
}
