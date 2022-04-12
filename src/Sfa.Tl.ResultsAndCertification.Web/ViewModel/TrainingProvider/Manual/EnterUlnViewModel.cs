using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class EnterUlnViewModel
    {
        // TODO: Delete
        public string EnterUln { get; set; }
        public bool IsNavigatedFromSearchLearnerRecordNotAdded { get; set; }
        public BackLinkModel BackLink => new BackLinkModel { RouteName = IsNavigatedFromSearchLearnerRecordNotAdded ? RouteConstants.SearchLearnerRecordNotAdded : RouteConstants.ManageLearnerRecordsDashboard };
    }
}
