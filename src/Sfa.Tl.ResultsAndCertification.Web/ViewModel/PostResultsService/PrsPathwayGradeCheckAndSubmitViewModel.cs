using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsPathwayGradeCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsPathwayGradeCheckAndSubmitViewModel : PrsBaseViewModel
    {
        public PrsPathwayGradeCheckAndSubmitViewModel()
        {
            UlnLabel = CheckAndSubmitContent.Title_Uln_Text;
            LearnerNameLabel = CheckAndSubmitContent.Title_Name_Text;
            DateofBirthLabel = CheckAndSubmitContent.Title_DateofBirth_Text;
            ProviderNameLabel = CheckAndSubmitContent.Title_Provider_Text;
            TlevelTitleLabel = CheckAndSubmitContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string PathwayTitle { get; set; }
        public int ResultId { get; set; }
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public bool IsGradeChanged { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public string SuccessBannerMessage { get { return string.Format(CheckAndSubmitContent.Success_Banner_Message, PathwayTitle); } }

        public SummaryItemModel SummaryOldGrade => new SummaryItemModel
        {
            Id = "oldGrade",
            Title = CheckAndSubmitContent.Title_Old_Grade,
            Value = OldGrade
        };

        public SummaryItemModel SummaryNewGrade => new SummaryItemModel
        {
            Id = "newGrade",
            Title = CheckAndSubmitContent.Title_New_Grade,
            Value = NewGrade,
            ActionText = CheckAndSubmitContent.Change_Link,
            HiddenActionText = CheckAndSubmitContent.Change_Link_Hidden_Text,
            RouteName = RouteConstants.PrsAppealUpdatePathwayGrade,
            RouteAttributes = new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ResultId, ResultId.ToString() },
                { Constants.IsChangeMode, true.ToString() }
            }
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = CheckAndSubmitContent.Title_ExamPeriod_Text,
            Value = PathwayAssessmentSeries
        };

        public override BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsGradeChanged ? RouteConstants.PrsAppealUpdatePathwayGrade : RouteConstants.PrsAppealOutcomePathwayGrade,
            RouteAttributes = IsGradeChanged ? new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ResultId, ResultId.ToString() },
                { Constants.IsChangeMode, false.ToString() }
            }
            : new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, AssessmentId.ToString() },
                { Constants.ResultId, ResultId.ToString() },
                { Constants.AppealOutcomeTypeId, ((int)AppealOutcomeType.GradeNotChanged).ToString() }
            }
        };
    }
}
