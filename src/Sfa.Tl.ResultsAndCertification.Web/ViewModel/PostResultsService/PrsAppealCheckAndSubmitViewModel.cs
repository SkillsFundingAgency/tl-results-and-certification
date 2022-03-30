using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using PrsAppealCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAppealCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAppealCheckAndSubmitViewModel : PrsBaseViewModel
    {
        public PrsAppealCheckAndSubmitViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsAppealCheckAndSubmitContent.Title_Name_Text;
            UlnLabel = PrsAppealCheckAndSubmitContent.Title_Uln_Text;
            DateofBirthLabel = PrsAppealCheckAndSubmitContent.Title_DateofBirth_Text;
            ProviderNameLabel = PrsAppealCheckAndSubmitContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = PrsAppealCheckAndSubmitContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = PrsAppealCheckAndSubmitContent.Title_TLevel_Text;
            CoreLabel = PrsAppealCheckAndSubmitContent.Title_Core_Text;
            ExamPeriodLabel = PrsAppealCheckAndSubmitContent.Title_ExamPeriod_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public bool IsGradeChanged { get; set; }
        public PrsStatus? PrsStatus { get; set; }

        public SummaryItemModel SummaryOldGrade => new SummaryItemModel
        {
            Id = "oldGrade",
            Title = PrsAppealCheckAndSubmitContent.Title_Old_Grade,
            Value = OldGrade
        };

        public SummaryItemModel SummaryNewGrade => new SummaryItemModel
        {
            Id = "newGrade",
            Title = PrsAppealCheckAndSubmitContent.Title_New_Grade,
            Value = NewGrade,
            ActionText = PrsAppealCheckAndSubmitContent.Change_Link,
            HiddenActionText = PrsAppealCheckAndSubmitContent.Change_Link_Hidden_Text,
            RouteName = RouteConstants.PrsAppealGradeChange,
            RouteAttributes = new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.IsAppealOutcomeJourney, "false" },
                { Constants.IsChangeMode, "true" }
            }
        };

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsGradeChanged ? RouteConstants.PrsAppealGradeChange : GetAppealOutcomeRouteName,
            RouteAttributes = IsGradeChanged ? new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.IsAppealOutcomeJourney, IsAppealOutcomeJourney.ToString().ToLowerInvariant() },
                { Constants.IsChangeMode, "false" }
            }
           : GetAppealOutcomeRouteAttributes
        };

        public string SuccessBannerMessage => string.Format(PrsAppealCheckAndSubmitContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : string.Empty);

        public string Banner_HeaderMesage => PrsAppealCheckAndSubmitContent.Banner_HeaderMessage_Appeal_Recorded;

        private bool IsAppealOutcomeJourney => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.BeingAppealed;
        private string GetAppealOutcomeRouteName => IsAppealOutcomeJourney ? RouteConstants.PrsAddAppealOutcome : RouteConstants.PrsAddAppealOutcomeKnown;

        private Dictionary<string, string> GetAppealOutcomeRouteAttributes
        {
            get
            {
                return IsAppealOutcomeJourney
                        ?
                        new Dictionary<string, string>
                        {
                            { Constants.ProfileId, ProfileId.ToString() },
                            { Constants.AssessmentId, AssessmentId.ToString() },
                            { Constants.ComponentType, ((int)ComponentType).ToString() },
                            { Constants.AppealOutcomeTypeId, ((int)AppealOutcomeType.GradeNotChanged).ToString() }
                        }
                        :
                        new Dictionary<string, string>
                        {
                            { Constants.ProfileId, ProfileId.ToString() },
                            { Constants.AssessmentId, AssessmentId.ToString() },
                            { Constants.ComponentType, ((int)ComponentType).ToString() },
                            { Constants.AppealOutcomeKnownTypeId, ((int)AppealOutcomeKnownType.GradeNotChanged).ToString() }
                        };
            }
        }
    }
}
