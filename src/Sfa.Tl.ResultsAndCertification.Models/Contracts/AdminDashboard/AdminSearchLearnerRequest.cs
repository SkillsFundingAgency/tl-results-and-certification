using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminSearchLearnerRequest
    {
        public string SearchKey { get; set; }

        public int? ProviderId { get; set; }

        public IList<int> SelectedAcademicYears { get; set; }

        public IList<int> SelectedAwardingOrganisations { get; set; }

        public int? PageNumber { get; set; }
    }
}