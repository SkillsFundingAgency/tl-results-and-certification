using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchRegistrationController : ControllerBase
    {
        private readonly ISearchRegistrationService _searchRegistrationService;

        public SearchRegistrationController(ISearchRegistrationService registrationService)
        {
            _searchRegistrationService = registrationService;
        }

        [HttpGet]
        [Route("GetSearchRegistrationFilters")]
        public Task<SearchRegistrationFilters> GetSearchRegistrationFiltersAsync()
            => _searchRegistrationService.GetSearchFiltersAsync();

        [HttpPost]
        [Route("SearchRegistrationDetails")]
        public Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request)
            => _searchRegistrationService.SearchRegistrationDetailsAsync(request);
    }
}