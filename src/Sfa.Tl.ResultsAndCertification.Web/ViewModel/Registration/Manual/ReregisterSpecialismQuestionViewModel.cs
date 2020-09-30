using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ReregisterSpecialismQuestionViewModel : SpecialismQuestionViewModel
    {
        public override BackLinkModel BackLink => new BackLinkModel 
        { 
            RouteName = RouteConstants.ReregisterProvider //TODO: direct to core. 
        };
    }
}
