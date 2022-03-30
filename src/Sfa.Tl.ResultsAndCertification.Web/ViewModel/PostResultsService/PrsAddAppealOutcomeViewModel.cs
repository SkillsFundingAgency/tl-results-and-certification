using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddAppealOutcomeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddAppealOutcome;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddAppealOutcomeViewModel : PrsBaseViewModel
    {
        public PrsAddAppealOutcomeViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAddAppealOutcomeContent.Title_Name_Text;
            UlnLabel = PrsAddAppealOutcomeContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddAppealOutcomeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddAppealOutcomeContent.Title_TLevel_Text;
            CoreLabel = PrsAddAppealOutcomeContent.Title_Core_Text;
            ExamPeriodLabel = PrsAddAppealOutcomeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddAppealOutcomeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsStatus? PrsStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddAppealOutcomeContent), ErrorMessageResourceName = "Validation_Message")]
        public AppealOutcomeType? AppealOutcome { get; set; }

        public bool IsValid => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.BeingAppealed;

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };

        public void SetOutcomeType(int? outcomeTypeId)
        {
            AppealOutcome = EnumExtensions.IsValidValue<AppealOutcomeType>(outcomeTypeId) ? (AppealOutcomeType?)outcomeTypeId : null;
        }

        public string SuccessBannerMessage => string.Format(PrsAddAppealOutcomeContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : string.Empty);

        public string Banner_HeaderMesage => PrsAddAppealOutcomeContent.Banner_HeaderMessage_Appeal_Withdrawn;
    }
}