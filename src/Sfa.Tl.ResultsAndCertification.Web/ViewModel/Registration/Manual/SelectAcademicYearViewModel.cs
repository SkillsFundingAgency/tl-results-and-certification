using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectAcademicYearViewModel
    {
        public string SelectedAcademicYear { get; set; }

        public IList<SelectListItem> AcademicYearSelectList => EnumExtensions.GetList<AcademicYear>()?.Select(e => new SelectListItem { Text = e.GetDisplayName(), Value = ((int)e).ToString() }).ToList();

        public bool IsValidAcademicYear => EnumExtensions.IsValidValue<AcademicYear>(SelectedAcademicYear);

        public bool HasSpecialismsSelected { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = HasSpecialismsSelected ? RouteConstants.AddRegistrationSpecialism : RouteConstants.AddRegistrationSpecialismQuestion,
                };
            }
        }
    }
}
