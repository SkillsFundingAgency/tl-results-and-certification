using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
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

        public bool IsChangeMode { get; set; }

        public BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : HasSpecialismsSelected ? RouteConstants.AddRegistrationSpecialisms : RouteConstants.AddRegistrationSpecialismQuestion };
    }
}