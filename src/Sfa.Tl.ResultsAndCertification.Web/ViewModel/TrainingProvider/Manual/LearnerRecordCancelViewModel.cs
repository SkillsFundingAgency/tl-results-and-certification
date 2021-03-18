using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordCancelViewModel
    {
        public string LearnerName { get; set; }

        public bool CancelLearnerRecord { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddLearnerRecordCheckAndSubmit };
    }
}
