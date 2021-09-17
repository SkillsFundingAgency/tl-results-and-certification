using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class SelectAcademicYearViewModel
    {
        public string SelectedAcademicYear { get; set; }

        public IEnumerable<AcademicYear> AcademicYears { get; set; }
        
        public IList<SelectListItem> AcademicYearSelectList => AcademicYears?.Select(e => new SelectListItem { Text = e.Name, Value = e.Year.ToString() }).ToList();

        public bool IsValidAcademicYear => !string.IsNullOrWhiteSpace(SelectedAcademicYear) && AcademicYears != null && AcademicYears.Any(a => a.Year == SelectedAcademicYear.ToInt());

        public bool HasSpecialismsSelected { get; set; }

        public bool IsChangeMode { get; set; }

        public virtual BackLinkModel BackLink => new BackLinkModel { RouteName = IsChangeMode ? RouteConstants.AddRegistrationCheckAndSubmit : HasSpecialismsSelected ? RouteConstants.AddRegistrationSpecialisms : RouteConstants.AddRegistrationSpecialismQuestion };
    }
}