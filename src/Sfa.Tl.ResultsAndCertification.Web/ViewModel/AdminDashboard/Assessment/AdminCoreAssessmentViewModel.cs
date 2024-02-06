using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.Linq;
using PathwayAssessments = Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminCoreAssessmentViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public string LearnerName { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string TlevelName { get; set; }

        public int StartYear { get; set; }

        public string DisplayStartYear { get; set; }

        public string PathwayDisplayName { get; set; }

        public IEnumerable<PathwayAssessments> PathwayAssessments { get; set; }

        public IEnumerable<PathwayAssessments> AvailablePathwayAssessment { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - StartYear > 4;

        public bool HasCoreAssessmentEntries { get; set; }

        public bool HasReachedAssessmentsThreashold => PathwayAssessments.Count() == Constants.AdminAssessmentEntryLimit &&
            !AvailablePathwayAssessment.Any();

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminAddCoreAssessmentEntry.Title_Learner_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminAddCoreAssessmentEntry.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminAddCoreAssessmentEntry.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminAddCoreAssessmentEntry.Title_TLevel_Text,
            Value = TlevelName
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "startyear",
            Title = AdminAddCoreAssessmentEntry.Title_StartYear_Text,
            Value = DisplayStartYear
        };
    }
}
