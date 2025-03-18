using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation
{
    public class AdminSelectAwardingOrganisationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(SelectAwardingOrganisation), ErrorMessageResourceName = "Validation_Message")]
        public long? SelectedAwardingOrganisationUkprn { get; set; }

        public AwardingOrganisationMetadata[] AwardingOrganisations { get; set; }

        public BreadcrumbModel Breadcrumb
            => new()
            {
                BreadcrumbItems = new List<BreadcrumbItem>
                {
                    new() { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.AdminHome },
                    new() { DisplayName = BreadcrumbContent.Select_Awarding_Organisation }
                }
            };
    }
}