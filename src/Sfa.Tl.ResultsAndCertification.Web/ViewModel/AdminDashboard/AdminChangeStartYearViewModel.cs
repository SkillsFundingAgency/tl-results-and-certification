using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ChangeStarYear = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.ChangeStartYear;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminChangeStartYearViewModel
    {
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int PathwayId { get; set; }
        //public int PathwayId { get { return RegistrationPathwayId; } set { RegistrationPathwayId = this.PathwayId; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int TlevelStartYear { get; set; }
        public int AcademicYear { get; set; }
        public string DisplayAcademicYear { get; set; }
        public List<int> AcademicStartYearsToBe { get; set; }
        public string Learner => $"{FirstName} {LastName}";
        public string LearnerRegistrationPathwayStatus { get; set; }

        public CalculationStatus OverallCalculationStatus { get; set; }



        [Required(ErrorMessageResourceType = typeof(ChangeStarYear), ErrorMessageResourceName = "Validation_Message")]
        public string AcademicYearTo { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
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
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ChangeStarYear.Title_TLevel_Text,
            Value = TlevelName
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = ChangeStarYear.Title_StartYear_Text,
            Value = DisplayAcademicYear
        };


        public bool IsOverallResultCalculated => OverallCalculationStatus == CalculationStatus.Completed || OverallCalculationStatus == CalculationStatus.CompletedAppealRaised || OverallCalculationStatus == CalculationStatus.CompletedRommRaised;

        public bool IsTlevelStartedSameAsStartYear => TlevelStartYear == AcademicYear;

        public bool IsLearnerWithdrawn => LearnerRegistrationPathwayStatus == nameof(RegistrationPathwayStatus.Withdrawn);

        public bool IsLearnerRegisteredFourYearsAgo => (DateTime.Now.Year - AcademicYear) > 4;

        public bool DisplayDevlopmentTicketMessage => IsOverallResultCalculated;

        public string StartYearCannotChangeMessage
        {
            get
            {
                if (IsLearnerWithdrawn) return ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Learner_Has_Been_Withdrawn;
                else if (IsOverallResultCalculated) return ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Overall_Result_Already_Calculated;
                else if (IsTlevelStartedSameAsStartYear) return ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Tlevel_Became_Available_This_Academic_Year;
                else if (IsLearnerRegisteredFourYearsAgo) return ChangeStartYear.Message_Start_Year_Cannot_Be_Changed_Learner_Started_Course_More_Than_4_Years;
                else return string.Empty;
            }
        }
    }
}