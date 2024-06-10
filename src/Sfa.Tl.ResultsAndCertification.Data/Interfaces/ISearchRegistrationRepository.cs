using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ISearchRegistrationRepository
    {
        Task<IList<FilterLookupData>> GetAcademicYearFiltersAsync(Func<DateTime> getSearchDate);

        Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request);
    }
}