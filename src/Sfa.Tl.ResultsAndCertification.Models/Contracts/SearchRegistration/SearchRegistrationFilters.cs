using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration
{
    public class SearchRegistrationFilters
    {
        public IList<FilterLookupData> AcademicYears { get; set; }
    }
}