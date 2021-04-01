using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class QueryEnglishAndMathsViewModel
    {
        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.LearnerRecordDetails
        };
    }
}
