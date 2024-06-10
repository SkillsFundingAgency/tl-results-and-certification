using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class SearchRegistrationDetailsViewModel
    {
        public int RegistrationProfileId { get; set; }

        public long Uln { get; set; }

        public string LearnerName { get; set; }

        public string Provider { get; set; }

        public string Core { get; set; }

        public string StartYear { get; set; }

        public RouteModel Route { get; set; }
    }
}