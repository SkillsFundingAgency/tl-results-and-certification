using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminDashboardLoader : IAdminDashboardLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AdminDashboardLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<AdminSearchLearnerFiltersViewModel> GetAdminSearchLearnerFiltersAsync()
        {
            var apiResponse = await _internalApiClient.GetAdminSearchLearnerFiltersAsync();
            return _mapper.Map<AdminSearchLearnerFiltersViewModel>(apiResponse);
        }
    }
}
