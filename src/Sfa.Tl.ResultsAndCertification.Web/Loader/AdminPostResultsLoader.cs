using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminPostResultsLoader : IAdminPostResultsLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public AdminPostResultsLoader(
            IResultsAndCertificationInternalApiClient internalApiClient,
            IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public Task<AdminOpenPathwayRommViewModel> GetAdminOpenPathwayRommAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAdminOpenRommAsync<AdminOpenPathwayRommViewModel>(registrationPathwayId, pathwayAssessmentId);

        public AdminOpenPathwayRommReviewChangesViewModel GetAdminOpenPathwayRommReviewChangesAsync(AdminOpenPathwayRommViewModel openPathwayRommViewModel)
            => _mapper.Map<AdminOpenPathwayRommReviewChangesViewModel>(openPathwayRommViewModel);

        public Task<bool> ProcessAdminOpenPathwayRommAsync(AdminOpenPathwayRommReviewChangesViewModel openPathwayRommReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenPathwayRommRequest>(openPathwayRommReviewChangesViewModel);
            return _internalApiClient.ProcessAdminOpenPathwayRommAsync(request);
        }

        public Task<AdminOpenSpecialismRommViewModel> GetAdminOpenSpecialismRommAsync(int registrationPathwayId, int specialismAssessmentId)
            => GetAdminOpenRommAsync<AdminOpenSpecialismRommViewModel>(registrationPathwayId, specialismAssessmentId);

        public AdminOpenSpecialismRommReviewChangesViewModel GetAdminOpenSpecialismRommReviewChangesAsync(AdminOpenSpecialismRommViewModel openSpecialismRommViewModel)
           => _mapper.Map<AdminOpenSpecialismRommReviewChangesViewModel>(openSpecialismRommViewModel);

        public Task<bool> ProcessAdminOpenSpecialismRommAsync(AdminOpenSpecialismRommReviewChangesViewModel openSpecialismRommReviewChangesViewModel)
        {
            var request = _mapper.Map<OpenSpecialismRommRequest>(openSpecialismRommReviewChangesViewModel);
            return _internalApiClient.ProcessAdminOpenSpecialismRommAsync(request);
        }

        private async Task<T> GetAdminOpenRommAsync<T>(int registrationPathwayId, int pathwayAssessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<T>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = pathwayAssessmentId;
            });
        }

        public Task<AdminAddCoreRommOutcomeViewModel> GetAdminAddPathwayRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAdminOpenRommAsync<AdminAddCoreRommOutcomeViewModel>(registrationPathwayId, pathwayAssessmentId);

        public Task<AdminAddSpecialismRommOutcomeViewModel> GetAdminAddSpecialismRommOutcomeAsync(int registrationPathwayId, int pathwayAssessmentId)
            => GetAdminOpenRommAsync<AdminAddSpecialismRommOutcomeViewModel>(registrationPathwayId, pathwayAssessmentId);
    }
}