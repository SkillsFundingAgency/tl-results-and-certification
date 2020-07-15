using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectSpecialismViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectSpecialism), ErrorMessageResourceName = "Validation_Select_Specialism_Required_Message")]
        public bool? HasSpecialismSelected => (PathwaySpecialisms?.Specialisms.Any(x => x.IsSelected) == true) ? true : (bool?)null;

        public PathwaySpecialismsViewModel PathwaySpecialisms { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationSpecialismQuestion
                };
            }
        }
    }
}
