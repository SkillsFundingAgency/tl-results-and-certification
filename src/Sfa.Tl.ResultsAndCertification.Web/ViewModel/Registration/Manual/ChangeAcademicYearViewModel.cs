using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ChangeAcademicYearContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.ChangeAcademicYear;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class ChangeAcademicYearViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public long AoUkprn { get; set; }
        public string Name { get; set; }
        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public int AcademicYear { get; set; }
        public int AcademicYearToBe { get; set; }
        public IList<AcademicYear> AcademicYears { get; set; }

        public bool HasActiveAssessmentResults { get; set; }

        [Required(ErrorMessageResourceType = typeof(ChangeAcademicYear), ErrorMessageResourceName = "Validation_Message")]
        public string AcademicYearChangeTo { get; set; }

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = ChangeAcademicYearContent.Title_Name_Text, Value = Name };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = ChangeAcademicYearContent.Title_Provider_Text, Value = ProviderDisplayName };
        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = ChangeAcademicYearContent.Title_Uln, Value = Uln.ToString() };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = ChangeAcademicYearContent.Title_Core_Text, Value = PathwayDisplayName };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = ChangeAcademicYearContent.Title_AcademicYear_Text, Value = GetAcademicYearName };

        public SummaryItemModel SummaryChangeAcademicYear => new()
        {
            Id = "changeacademicyear",
            Title = ReviewChangeAcademicYear.Title_AcademicYear_Text,
            Value = GetAcademicYearName,
            Value2 = $"{AcademicYearChangeTo}/{(int.TryParse(AcademicYearChangeTo, out int academicYearToInt) ? (academicYearToInt + 1).ToString().Substring(2) : default)}"
        };


        public string GetAcademicYearName => AcademicYears?.FirstOrDefault(a => a.Year == AcademicYear)?.Name;

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.RegistrationDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
