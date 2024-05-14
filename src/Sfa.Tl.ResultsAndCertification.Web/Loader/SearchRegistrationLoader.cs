using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class SearchRegistrationLoader : ISearchRegistrationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public SearchRegistrationLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<SearchRegistrationViewModel> CreateSearchRegistration(SearchRegistrationType type)
        {
            SearchRegistrationFiltersViewModel filters = await GetSearchRegistrationFiltersAsync();
            return new SearchRegistrationViewModel(type, filters);
        }

        public async Task<SearchRegistrationDetailsListViewModel> GetSearchRegistrationDetailsListAsync(long aoUkprn, SearchRegistrationType type, SearchRegistrationCriteriaViewModel searchCriteria)
        {
            var searchRegistrationRequest = _mapper.Map<SearchRegistrationRequest>(searchCriteria, opt => opt.Items["aoUkprn"] = aoUkprn);
            PagedResponse<SearchRegistrationDetail> apiResponse = await _internalApiClient.SearchRegistrationDetailsAsync(searchRegistrationRequest);

            return _mapper.Map<SearchRegistrationDetailsListViewModel>(apiResponse, opt => opt.Items["search-type"] = type);
        }

        private async Task<SearchRegistrationFiltersViewModel> GetSearchRegistrationFiltersAsync()
        {
            SearchRegistrationFilters apiResponse = await _internalApiClient.GetSearchRegistrationFiltersAsync();
            return _mapper.Map<SearchRegistrationFiltersViewModel>(apiResponse);
        }
    }
}