using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddAppealOutcomeKnownContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddAppealOutcomeKnown;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddAppealOutcomeKnownViewModel : PrsBaseViewModel
    {
        public PrsAddAppealOutcomeKnownViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAddAppealOutcomeKnownContent.Title_Name_Text;
            UlnLabel = PrsAddAppealOutcomeKnownContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddAppealOutcomeKnownContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddAppealOutcomeKnownContent.Title_TLevel_Text;
            CoreLabel = PrsAddAppealOutcomeKnownContent.Title_Core_Text;
            SpecialismLabel = PrsAddAppealOutcomeKnownContent.Title_Specialism_Text;
            ExamPeriodLabel = PrsAddAppealOutcomeKnownContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddAppealOutcomeKnownContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddAppealOutcomeKnownContent), ErrorMessageResourceName = "Validation_Message")]
        public AppealOutcomeKnownType? AppealOutcome { get; set; }

        public bool IsValid => (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed) && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsAddAppeal,
            RouteAttributes = new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.IsBack, "true" }
            }
        };

        public void SetOutcomeType(int? outcomeKnownTypeId)
        {
            AppealOutcome = EnumExtensions.IsValidValue<AppealOutcomeKnownType>(outcomeKnownTypeId) ? (AppealOutcomeKnownType?)outcomeKnownTypeId : null;
        }

        public string SuccessBannerMessage => string.Format(PrsAddAppealOutcomeKnownContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : SpecialismDisplayName);

        public string Banner_HeaderMesage => PrsAddAppealOutcomeKnownContent.Banner_HeaderMessage_Appeal_Recorded;
    }
}