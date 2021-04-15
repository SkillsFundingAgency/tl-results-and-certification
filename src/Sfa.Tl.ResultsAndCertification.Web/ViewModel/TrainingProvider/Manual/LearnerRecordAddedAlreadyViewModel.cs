using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordAddedAlreadyViewModel
    {
        public int ProfileId { get; set; }
        public string Uln { get; set; }

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.EnterUniqueLearnerNumber
        };
    }
}
