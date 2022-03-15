using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddRommOutcomeKnownCoreGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommOutcomeKnownCoreGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddRommOutcomeKnownCoreGradeViewModel : PrsBaseViewModel
    {
        public PrsAddRommOutcomeKnownCoreGradeViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_Name_Text;
            UlnLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_TLevel_Text;
            CoreLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_Core_Text;
            ExamPeriodLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddRommOutcomeKnownCoreGradeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }        

        [Required(ErrorMessageResourceType = typeof(PrsAddRommOutcomeKnownCoreGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public RommOutcomeKnownType? RommOutcome { get; set; }

        public bool IsValid => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && CommonHelper.IsRommAllowed(RommEndDate);

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsAddRommCoreGrade,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() }, { Constants.IsBack, "true" } }
        };

        public void SetOutcomeType(int? outcomeKnownTypeId)
        {
            RommOutcome = EnumExtensions.IsValidValue<RommOutcomeKnownType>(outcomeKnownTypeId) ? (RommOutcomeKnownType?)outcomeKnownTypeId : null;
        }

        public string SuccessBannerMessage 
        { 
            get 
            { 
                return string.Format(PrsAddRommOutcomeKnownCoreGradeContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : string.Empty);
            }
        }

        public string Banner_HeaderMesage => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) 
                                          ? PrsAddRommOutcomeKnownCoreGradeContent.Banner_HeaderMessage_Romm_Recorded : string.Empty;
    }
}
