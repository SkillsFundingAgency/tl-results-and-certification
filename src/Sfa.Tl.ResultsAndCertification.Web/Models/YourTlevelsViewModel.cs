using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Models
{
    public class YourTlevelsViewModel
    {
        public int RouteId { get; internal set; }
        public int PathId { get; internal set; }
        public string TLevelDescription { get; internal set; }
        public string TLevelStatus { get; internal set; }
    }
}
