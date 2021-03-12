using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider
{
    public class EnterUlnViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.EnterUniqueLearnerReference), ErrorMessageResourceName = "Uln_Required_Validation_Message")]
        [RegularExpression(@"^\d{10}$", ErrorMessageResourceType = typeof(ErrorResource.EnterUniqueLearnerReference), ErrorMessageResourceName = "Uln_Not_Valid_Validation_Message")]
        public string EnterUln { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.ManageLearnerRecordsDashboard };
    }
}
