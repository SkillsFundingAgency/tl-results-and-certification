using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaUlnNotWithdrawnViewModel
    {
        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.RequestSoaUniqueLearnerNumber
        };
    }
}