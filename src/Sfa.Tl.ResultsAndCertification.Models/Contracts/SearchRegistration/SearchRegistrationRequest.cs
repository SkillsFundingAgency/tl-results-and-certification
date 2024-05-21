using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration
{
    public class SearchRegistrationRequest
    {
        public long AoUkprn { get; set; }

        public string SearchKey { get; set; }

        public int? ProviderId { get; set; }

        public IList<int> SelectedAcademicYears { get; set; }

        public int? PageNumber { get; set; }
    }
}