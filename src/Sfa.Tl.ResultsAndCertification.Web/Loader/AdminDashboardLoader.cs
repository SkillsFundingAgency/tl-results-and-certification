using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
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

        public async Task<LearnerRecordViewModel> GetLearnerRecordAsync<LearnerRecordViewModel>(int profileId)
        {
            var response = await _internalApiClient.GetLearnerRecordAsync(profileId);
            return _mapper.Map<LearnerRecordViewModel>(response);
        }

        
}
}
