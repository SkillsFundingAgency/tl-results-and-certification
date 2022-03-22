using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddRommOutcomeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommOutcome;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddRommOutcomeViewModel : PrsBaseViewModel
    {
        public PrsAddRommOutcomeViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAddRommOutcomeContent.Title_Name_Text;
            UlnLabel = PrsAddRommOutcomeContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddRommOutcomeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddRommOutcomeContent.Title_TLevel_Text;
            CoreLabel = PrsAddRommOutcomeContent.Title_Core_Text;
            SpecialismLabel = PrsAddRommOutcomeContent.Title_Specialism_Text;
            ExamPeriodLabel = PrsAddRommOutcomeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddRommOutcomeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddRommOutcomeContent), ErrorMessageResourceName = "Validation_Message")]
        public RommOutcomeType? RommOutcome { get; set; }

        public bool IsValid => (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.UnderReview);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };

        public void SetOutcomeType(int? outcomeTypeId)
        {
            RommOutcome = EnumExtensions.IsValidValue<RommOutcomeType>(outcomeTypeId) ? (RommOutcomeType?)outcomeTypeId : null;
        }

        public string SuccessBannerMessage
        {
            get
            {
                return string.Format(PrsAddRommOutcomeContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : string.Empty);
            }
        }

        public string Banner_HeaderMesage => PrsAddRommOutcomeContent.Banner_HeaderMessage_Romm_Withdrawn;
    }
}
