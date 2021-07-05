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
        public string PathwayName { get; set; }
        public string OldGrade { get; set; }
        public string NewGrade { get; set; }
        public string PathwayTitle { get { return string.Format(CheckAndSubmitContent.Heading_Pathway_Title, PathwayName); } }

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
            RouteName = "TODO",
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }}, 
            HiddenActionText = CheckAndSubmitContent.Change_Link_Hidden_Text
        };

        public override BackLinkModel BackLink => new BackLinkModel
        {
            // TODO: conditional route 
            RouteName = RouteConstants.PrsSearchLearner,
            RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } }
        };
    }
}
