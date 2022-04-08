using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAppealGradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAppealGradeChange;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAppealGradeChangeViewModel : PrsBaseViewModel
    {
        public PrsAppealGradeChangeViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAppealGradeChangeContent.Title_Name_Text;
            UlnLabel = PrsAppealGradeChangeContent.Title_Uln_Text;
            DateofBirthLabel = PrsAppealGradeChangeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAppealGradeChangeContent.Title_TLevel_Text;
            CoreLabel = PrsAppealGradeChangeContent.Title_Core_Text;
            SpecialismLabel = PrsAppealGradeChangeContent.Title_Specialism_Text;
            ExamPeriodLabel = PrsAppealGradeChangeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAppealGradeChangeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAppealGradeChangeContent), ErrorMessageResourceName = "Validation_Message")]
        public string SelectedGradeCode { get; set; }

        public List<LookupViewModel> Grades { get; set; }

        public bool IsAppealOutcomeJourney { get; set; }

        public bool IsChangeMode { get; set; }

        public bool IsValid => (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(AppealEndDate))
                             || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.BeingAppealed;

        public override BackLinkModel BackLink => new()
        {
            RouteName = GetRouteName,
            RouteAttributes = GetRouteAttributes
        };

        private string GetRouteName => IsChangeMode ? RouteConstants.PrsAppealCheckAndSubmit : GetAppealOutcomeJourneyRoute;

        private string GetAppealOutcomeJourneyRoute => IsAppealOutcomeJourney ? RouteConstants.PrsAddAppealOutcome : RouteConstants.PrsAddAppealOutcomeKnown;

        private Dictionary<string, string> GetRouteAttributes =>
            IsChangeMode ? null :
            IsAppealOutcomeJourney ?
            new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.AppealOutcomeTypeId, ((int)AppealOutcomeType.GradeChanged).ToString() }
            }
            :
            new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.AppealOutcomeKnownTypeId, ((int)AppealOutcomeKnownType.GradeChanged).ToString() }
            };
    }
}