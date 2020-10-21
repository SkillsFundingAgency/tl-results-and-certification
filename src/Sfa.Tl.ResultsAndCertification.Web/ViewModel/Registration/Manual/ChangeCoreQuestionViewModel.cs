using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ChangeCoreQuestionViewModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.ChangeCoreQuestion), ErrorMessageResourceName = "Select_ChangeCoreQuestion_Validation_Message")]
        public bool? CanChangeCore { get; set; }

        public string ProviderDisplayName { get; set; }

        public string CoreDisplayName { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ChangeRegistrationProvider,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsBack, "true" } }
                };
            }
        }
    }
}
