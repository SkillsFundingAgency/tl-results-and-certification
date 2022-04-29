﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class PlacementHoursViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Required_Validation_Message")]
        [RegularExpression(Constants.IpSpecialConsiderationHoursRegex, ErrorMessageResourceType = typeof(ErrorResource.IpSpecialConsiderationHours), ErrorMessageResourceName = "Hours_Must_Be_Between_0_1000")]
        public string Hours { get; set; }

        public virtual BackLinkModel BackLink => new()
        {
            RouteName = "TODO"
        };
    }
}
