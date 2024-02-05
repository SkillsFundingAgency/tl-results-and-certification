using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver
{
    public class AdminAssessmentIsResultChangeAllowedResolver : IValueResolver<Assessment, AdminAssessmentViewModel, bool>
    {
        private readonly ISystemProvider _systemProvider;

        private readonly AdminAssessmentResultStatus[] _trueResultStatuses = new[]
        {
            AdminAssessmentResultStatus.OpenRommAllowed,
            AdminAssessmentResultStatus.OpenAppealAllowed
        };

        public AdminAssessmentIsResultChangeAllowedResolver(ISystemProvider systemProvider)
        {
            _systemProvider = systemProvider;
        }

        public bool Resolve(Assessment source, AdminAssessmentViewModel destination, bool destMember, ResolutionContext context)
        {
            AdminAssessmentResultStatus status = source.GetAdminAssessmentResultStatus(_systemProvider.Today);
            return _trueResultStatuses.Contains(status);
        }
    }
}