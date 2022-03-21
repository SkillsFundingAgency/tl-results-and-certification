using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddRommOutcomeKnownContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommOutcomeKnown;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddRommOutcomeKnownViewModel : PrsBaseViewModel
    {
        public PrsAddRommOutcomeKnownViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAddRommOutcomeKnownContent.Title_Name_Text;
            UlnLabel = PrsAddRommOutcomeKnownContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddRommOutcomeKnownContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddRommOutcomeKnownContent.Title_TLevel_Text;
            CoreLabel = PrsAddRommOutcomeKnownContent.Title_Core_Text;
            SpecialismLabel = PrsAddRommOutcomeKnownContent.Title_Specialism_Text;
            ExamPeriodLabel = PrsAddRommOutcomeKnownContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddRommOutcomeKnownContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }        

        [Required(ErrorMessageResourceType = typeof(PrsAddRommOutcomeKnownContent), ErrorMessageResourceName = "Validation_Message")]
        public RommOutcomeKnownType? RommOutcome { get; set; }

        public bool IsValid => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && CommonHelper.IsRommAllowed(RommEndDate);

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsAddRomm,
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
            RommOutcome = EnumExtensions.IsValidValue<RommOutcomeKnownType>(outcomeKnownTypeId) ? (RommOutcomeKnownType?)outcomeKnownTypeId : null;
        }

        public string SuccessBannerMessage 
        { 
            get 
            { 
                return string.Format(PrsAddRommOutcomeKnownContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : SpecialismDisplayName);
            }
        }

        public string Banner_HeaderMesage => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) 
                                          ? PrsAddRommOutcomeKnownContent.Banner_HeaderMessage_Romm_Recorded : string.Empty;
    }
}
