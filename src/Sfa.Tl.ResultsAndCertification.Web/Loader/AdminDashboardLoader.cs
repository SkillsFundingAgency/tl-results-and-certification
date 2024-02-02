using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
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
            AdminSearchLearnerFilters apiResponse = await _internalApiClient.GetAdminSearchLearnerFiltersAsync();
            return _mapper.Map<AdminSearchLearnerFiltersViewModel>(apiResponse);
        }

        public async Task<AdminSearchLearnerDetailsListViewModel> GetAdminSearchLearnerDetailsListAsync(AdminSearchLearnerCriteriaViewModel adminSearchCriteria)
        {
            var adminSearchLearnerRequest = _mapper.Map<AdminSearchLearnerRequest>(adminSearchCriteria);
            PagedResponse<AdminSearchLearnerDetail> apiResponse = await _internalApiClient.GetAdminSearchLearnerDetailsAsync(adminSearchLearnerRequest);
            return _mapper.Map<AdminSearchLearnerDetailsListViewModel>(apiResponse);
        }

        public async Task<TLearnerRecordViewModel> GetAdminLearnerRecordAsync<TLearnerRecordViewModel>(int registrationPathwayId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);

            TLearnerRecordViewModel response = _mapper.Map<TLearnerRecordViewModel>(learnerRecord, opt =>
            {
                opt.Items[Constants.RegistrationPathwayId] = learnerRecord.RegistrationPathwayId;
            });

            return response;
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearViewModel reviewChangeStartYearViewModel)
        {
            var reviewChangeStartYearRequest = _mapper.Map<ReviewChangeStartYearRequest>(reviewChangeStartYearViewModel);
            return await _internalApiClient.ProcessChangeStartYearAsync(reviewChangeStartYearRequest);
        }
    }
}