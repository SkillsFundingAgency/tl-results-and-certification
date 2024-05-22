using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class SearchRegistrationService : ISearchRegistrationService
    {
        private readonly ISearchRegistrationRepository _searchRegistrationRepository;

        public SearchRegistrationService(ISearchRegistrationRepository searchRegistrationRepository)
        {
            _searchRegistrationRepository = searchRegistrationRepository;
        }

        public async Task<SearchRegistrationFilters> GetSearchFiltersAsync()
            => new SearchRegistrationFilters
            {
                AcademicYears = await _searchRegistrationRepository.GetAcademicYearFiltersAsync(() => DateTime.UtcNow.Date)
            };

        public Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request)
            => _searchRegistrationRepository.SearchRegistrationDetailsAsync(request);
    }
}
