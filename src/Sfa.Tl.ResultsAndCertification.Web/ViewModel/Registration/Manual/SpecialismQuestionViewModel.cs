using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SpecialismQuestionViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SpecialismQuestion), ErrorMessageResourceName = "Validation_Select_Yes_Required_Message")]
        public bool? HasLearnerDecidedSpecialism { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationCore,
                };
            }
        }
    }
}
