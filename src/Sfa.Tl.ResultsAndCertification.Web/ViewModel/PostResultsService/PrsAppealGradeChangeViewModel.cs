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

        public bool IsValid => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = GetRouteName,
            RouteAttributes = GetRouteAttributes
        };

        private string GetRouteName => GetAppealOutcomeJourneyRoute;

        private string GetAppealOutcomeJourneyRoute => RouteConstants.PrsAddAppealOutcomeKnown;

        private Dictionary<string, string> GetRouteAttributes =>
            IsAppealOutcomeJourney ?
            new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.AppealOutcomeTypeId, ((int)AppealOutcomeKnownType.GradeChanged).ToString() }
            }
            :
            null;
    }
}
