using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class PathwaySpecialismsViewModel
    {
        public int PathwayId { get; set; }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public IList<SpecialismDetailsViewModel> Specialisms { get; set; }
    }
}
