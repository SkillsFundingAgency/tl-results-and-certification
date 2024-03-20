using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
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

        public Task<AdminOpenSpecialismRommViewModel> GetAdminOpenSpecialismRommAsync(int registrationPathwayId, int specialismAssessmentId)
            => GetAdminOpenRommAsync<AdminOpenSpecialismRommViewModel>(registrationPathwayId, specialismAssessmentId);

        private async Task<T> GetAdminOpenRommAsync<T>(int registrationPathwayId, int pathwayAssessmentId)
        {
            AdminLearnerRecord learnerRecord = await _internalApiClient.GetAdminLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<T>(learnerRecord, opt =>
            {
                opt.Items[Constants.AssessmentId] = pathwayAssessmentId;
            });
        }
    }
}