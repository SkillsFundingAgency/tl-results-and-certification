using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using NoAssessmentEntryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultNoAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultNoAssessmentEntryViewModel : ResultsBaseViewModel
    {
        public ResultNoAssessmentEntryViewModel()
        {
            UlnLabel = NoAssessmentEntryContent.Title_Uln_Text;
            LearnerNameLabel = NoAssessmentEntryContent.Title_Name_Text;
            DateofBirthLabel = NoAssessmentEntryContent.Title_DateofBirth_Text;
            ProviderNameLabel = NoAssessmentEntryContent.Title_Provider_Text;
            TlevelTitleLabel = NoAssessmentEntryContent.Title_TLevel_Text;
        }

        public override BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.SearchResults, RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } } };
    }
}
