using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider
{
    public class ProviderTlevelDetailsViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public string TlevelTitle { get; set; }
        public int TlProviderId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ErrorResource.RemoveProviderTlevel), ErrorMessageResourceName = "Select_RemoveProviderTlevel_Validation_Message")]
        public bool? CanRemoveTlevel { get; set; }
        public bool ShowBackToProvidersLink { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.ProviderTlevels,
                    RouteAttributes = new System.Collections.Generic.Dictionary<string, string> { { "providerId", TlProviderId.ToString() } }
                };
            }
        }
    }
}
