﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
//using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using ChangeStarYear = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.ChangeStartYear;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminChangeStartYearViewModel
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string Provider { get; set; }
        public int Ukprn { get; set; }
        public string Tlevel { get; set; }
        public int TlevelStartYear { get; set; }
        public int AcademicYear { get; set; }
        public string DisplayAcademicYear { get; set; }
        public List<int> AcademicStartYearsToBe { get; set; }
        public string Learner => $"{FirstName} {LastName}";

        [Required(ErrorMessageResourceType = typeof(ChangeStarYear), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsAcademicYearToBeSelected { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.SearchLearnerRecord
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ChangeStarYear.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ChangeStarYear.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ChangeStarYear.Title_Provider_Text,
            Value = $"{Provider} ({Ukprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ChangeStarYear.Title_TLevel_Text,
            Value = Tlevel
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = ChangeStarYear.Title_StartYear_Text,
            Value = DisplayAcademicYear
        };
    }
}