using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class TLevelDetailsViewModel
    {
        public TLevelDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        public int PathwayId { get; set; }
        public string PageTitle { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public bool ShowSomethingIsNotRight { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
    }
}
