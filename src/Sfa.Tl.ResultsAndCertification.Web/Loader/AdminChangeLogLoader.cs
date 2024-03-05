using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminChangeLogLoader : IAdminChangeLogLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AdminChangeLogLoader(
            IResultsAndCertificationInternalApiClient internalApiClient,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<AdminSearchChangeLogViewModel> SearchChangeLogsAsync(string searchKey, int? pageNumber)
        {
            AdminSearchChangeLogRequest request = new()
            {
                SearchKey = searchKey,
                PageNumber = pageNumber
            };

            PagedResponse<AdminSearchChangeLog> apiResponse = await _internalApiClient.SearchChangeLogsAsync(request);
            return _mapper.Map<AdminSearchChangeLogViewModel>(apiResponse);
        }
    }
}