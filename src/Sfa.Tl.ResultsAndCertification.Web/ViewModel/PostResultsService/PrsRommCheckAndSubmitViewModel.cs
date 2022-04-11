using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using PrsRommCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsRommCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsRommCheckAndSubmitViewModel : PrsBaseViewModel
    {
        public PrsRommCheckAndSubmitViewModel()
        {
            // Base Profile Summary
            LearnerNameLabel = PrsRommCheckAndSubmitContent.Title_Name_Text;
            UlnLabel = PrsRommCheckAndSubmitContent.Title_Uln_Text;
            DateofBirthLabel = PrsRommCheckAndSubmitContent.Title_DateofBirth_Text;
            ProviderNameLabel = PrsRommCheckAndSubmitContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = PrsRommCheckAndSubmitContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = PrsRommCheckAndSubmitContent.Title_TLevel_Text;
            CoreLabel = PrsRommCheckAndSubmitContent.Title_Core_Text;
            ExamPeriodLabel = PrsRommCheckAndSubmitContent.Title_ExamPeriod_Text;
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
            Title = PrsRommCheckAndSubmitContent.Title_Old_Grade,
            Value = OldGrade
        };

        public SummaryItemModel SummaryNewGrade => new SummaryItemModel
        {
            Id = "newGrade",
            Title = PrsRommCheckAndSubmitContent.Title_New_Grade,
            Value = NewGrade,
            ActionText = PrsRommCheckAndSubmitContent.Change_Link,
            HiddenActionText = PrsRommCheckAndSubmitContent.Change_Link_Hidden_Text,
            RouteName = RouteConstants.PrsRommGradeChange,
            RouteAttributes = new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.IsRommOutcomeJourney, "false" },
                { Constants.IsChangeMode, "true" }
            }
        };

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsGradeChanged ? RouteConstants.PrsRommGradeChange : GetRommOutcomeRouteName,
            RouteAttributes = IsGradeChanged ? new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ComponentType, ((int)ComponentType).ToString() },
                { Constants.IsRommOutcomeJourney, IsRommOutcomeJourney.ToString().ToLowerInvariant() },
                { Constants.IsChangeMode, "false" }
            }
           : GetRommOutcomeRouteAttributes
        };

        public string SuccessBannerMessage => string.Format(PrsRommCheckAndSubmitContent.Banner_Message, LearnerName, ExamPeriod, ComponentType == ComponentType.Core ? CoreDisplayName : SpecialismDisplayName);

        public string Banner_HeaderMesage => PrsRommCheckAndSubmitContent.Banner_HeaderMessage_Romm_Recorded;

        private bool IsRommOutcomeJourney => PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.UnderReview;
        private string GetRommOutcomeRouteName => IsRommOutcomeJourney ? RouteConstants.PrsAddRommOutcome : RouteConstants.PrsAddRommOutcomeKnown;

        private Dictionary<string, string> GetRommOutcomeRouteAttributes
        {
            get
            {
                return IsRommOutcomeJourney
                        ?
                        new Dictionary<string, string>
                        {
                            { Constants.ProfileId, ProfileId.ToString() },
                            { Constants.AssessmentId, AssessmentId.ToString() },
                            { Constants.ComponentType, ((int)ComponentType).ToString() },
                            { Constants.RommOutcomeTypeId, ((int)RommOutcomeType.GradeNotChanged).ToString() }
                        }
                        :
                        new Dictionary<string, string>
                        {
                            { Constants.ProfileId, ProfileId.ToString() },
                            { Constants.AssessmentId, AssessmentId.ToString() },
                            { Constants.ComponentType, ((int)ComponentType).ToString() },
                            { Constants.RommOutcomeKnownTypeId, ((int)RommOutcomeKnownType.GradeNotChanged).ToString() }
                        };
            }
        }
    }
}