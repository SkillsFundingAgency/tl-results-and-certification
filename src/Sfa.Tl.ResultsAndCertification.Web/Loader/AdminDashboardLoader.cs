using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Linq;
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
            AdminSearchLearnerFilters apiResponse = await _internalApiClient.GetAdminSearchLearnerFiltersAsync();
            return _mapper.Map<AdminSearchLearnerFiltersViewModel>(apiResponse);
        }

        public async Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria)
        {
            var request = new AdminSearchLearnerRequest
            {
                SearchKey = adminSearchCriteria.SearchKey,
                PageNumber = adminSearchCriteria.PageNumber,
                SelectedAcademicYears = adminSearchCriteria.SearchLearnerFilters?.AcademicYears?.Where(p => p.IsSelected).Select(p => p.Id).ToList(),
                SelectedAwardingOrganisations = adminSearchCriteria.SearchLearnerFilters?.AwardingOrganisations?.Where(p => p.IsSelected).Select(p => p.Id).ToList()
            };

            PagedResponse<AdminSearchLearnerDetail> apiResponse = await _internalApiClient.GetAdminSearchLearnerDetailsAsync(request);
            return _mapper.Map<AdminSearchLearnerDetailsListViewModel>(apiResponse);
        }


        public async Task<LearnerRecordViewModel> GetAdminLearnerRecordAsync<LearnerRecordViewModel>(int pathwayId)
        {
            var response = await _internalApiClient.GetAdminLearnerRecordAsync(pathwayId);
            return _mapper.Map<LearnerRecordViewModel>(response);
        }
    }
}
