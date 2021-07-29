using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultWithdrawnViewModel : ResultsBaseViewModel
    {
        public ResultWithdrawnViewModel()
        {
            UlnLabel = ResultDetailsContent.Title_Uln_Text;
            LearnerNameLabel = ResultDetailsContent.Title_Name_Text;
            DateofBirthLabel = ResultDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = ResultDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = ResultDetailsContent.Title_TLevel_Text;
        }

        public override BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.SearchResults, RouteAttributes = new Dictionary<string, string> { { Constants.PopulateUln, true.ToString() } } };
    }
}
