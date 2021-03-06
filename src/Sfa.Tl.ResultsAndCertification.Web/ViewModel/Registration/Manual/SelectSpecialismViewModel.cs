﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;
namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectSpecialismViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectSpecialisms), ErrorMessageResourceName = "Validation_Select_Specialism_Required_Message")]
        public bool? HasSpecialismSelected => (PathwaySpecialisms?.Specialisms.Any(x => x.Code == SelectedSpecialismCode) == true) ? true : (bool?)null;
        public PathwaySpecialismsViewModel PathwaySpecialisms { get; set; }
        public bool IsChangeMode { get; set; }
        public bool IsChangeModeFromSpecialismQuestion { get; set; }
        public string SelectedSpecialismCode { get; set; }
        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = (IsChangeMode && !IsChangeModeFromSpecialismQuestion) ?
            RouteConstants.AddRegistrationCheckAndSubmit : RouteConstants.AddRegistrationSpecialismQuestion,
            RouteAttributes = (IsChangeMode && IsChangeModeFromSpecialismQuestion) ? new Dictionary<string, string> { { Constants.IsChangeMode, "true" } } : null
        };
    }
}