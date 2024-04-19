using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface ISearchRegistrationService
    {
        Task<SearchRegistrationFilters> GetSearchFiltersAsync();

        Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request);
    }
}