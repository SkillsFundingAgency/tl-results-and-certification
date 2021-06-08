using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestSoaCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;
using EnglishAndMathsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsStatus;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class SoaLearnerRecordDetailsViewModel
    {
        public bool IsValid { get { return IsLearnerRegistered && !IsNotWithdrawn && IsIndustryPlacementAdded && !(HasPathwayResult == false && !IsIndustryPlacementCompleted); } }
        public bool IsRequestedAlready { get { return LastRequestedOn.HasValue && LastRequestedOn >=  DateTime.Now.AddDays(-28); } } 
        // TODO: above -28 need to be dynamic from config. 

        //Learner's registration details
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }

        //Learner's technical qualification details
        public string TlevelTitle { get; set; }
        public int RegistrationPathwayId { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public string PathwayGrade { get; set; }
        public string SpecialismDisplayName { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismCode { get; set; }
        public string SpecialismGrade { get; set; }
        public DateTime? LastRequestedOn { get; set; }

        //Learner's T level component achievements
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool? IsSendLearner { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }

        // Provider Organisation's postal address
        public AddressViewModel ProviderAddress { get; set; }

        // Validation properties
        public bool HasPathwayResult { get; set; }
        public bool IsIndustryPlacementAdded { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsNotWithdrawn { get; set; }
        public bool IsIndustryPlacementCompleted { get; set; }

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = RequestSoaCheckAndSubmitContent.Title_Uln_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = RequestSoaCheckAndSubmitContent.Title_Name_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = RequestSoaCheckAndSubmitContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryProvider => new SummaryItemModel
        {
            Id = "providername",
            Title = RequestSoaCheckAndSubmitContent.Title_Provider_Text,
            Value = ProviderDisplayName
        };

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = RequestSoaCheckAndSubmitContent.Title_Tlevel_Title_Text,
            Value = TlevelTitle
        };

        public SummaryItemModel SummaryCoreCode => new SummaryItemModel
        {
            Id = "corecode",
            Title = RequestSoaCheckAndSubmitContent.Title_Core_Code_Text,
            Value = string.Format(RequestSoaCheckAndSubmitContent.Core_Code_Value, PathwayDisplayName, PathwayGrade),
            IsRawHtml = true
        };

        public SummaryItemModel SummarySpecialismCode => new SummaryItemModel
        {
            Id = "specialismcode",
            Title = RequestSoaCheckAndSubmitContent.Title_Occupational_Specialism_Text,
            Value = string.Format(RequestSoaCheckAndSubmitContent.Occupational_Specialism_Value, SpecialismDisplayName, SpecialismGrade),
            IsRawHtml = true
        };

        public SummaryItemModel SummaryEnglishAndMaths => new SummaryItemModel
        {
            Id = "englishandmaths",
            Title = RequestSoaCheckAndSubmitContent.Title_English_And_Maths_Text,
            Value = GetEnglishAndMathsStatusDisplayText
        };

        public SummaryItemModel SummaryIndustryPlacement => new SummaryItemModel
        {
            Id = "industryplacement",
            Title = RequestSoaCheckAndSubmitContent.Title_Industry_Placement_Text,
            Value = GetIndustryPlacementDisplayText
        };

        public SummaryItemModel SummaryDepartment => new SummaryItemModel
        {
            Id = "department",
            Title = RequestSoaCheckAndSubmitContent.Title_Department_Text,
            Value = ProviderAddress?.DepartmentName
        };

        public SummaryItemModel SummaryAddress => new SummaryItemModel
        {
            Id = "address",
            Title = RequestSoaCheckAndSubmitContent.Title_Organisation_Address_Text,
            Value = string.Format(RequestSoaCheckAndSubmitContent.Organisation_Address_Value, FormatedAddress),
            IsRawHtml = true
        };

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Request_Statement_Of_Achievement, RouteName = RouteConstants.RequestStatementOfAchievement },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.RequestSoaUniqueLearnerNumber },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Check_Learner_Details }
                    }
                };
            }
        }

        private string FormatedAddress
        {
            get
            {
                var addressLines = new List<string> { ProviderAddress?.OrganisationName, ProviderAddress?.AddressLine1, ProviderAddress?.AddressLine2, ProviderAddress?.Town, ProviderAddress?.Postcode };
                return string.Join(RequestSoaCheckAndSubmitContent.Html_Line_Break, addressLines.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }

        public string GetIndustryPlacementDisplayText
        {
            get
            {
                return IndustryPlacementStatus switch
                {
                    IpStatus.Completed => IndustryPlacementStatusContent.Completed_Display_Text,
                    IpStatus.CompletedWithSpecialConsideration => IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text,
                    IpStatus.NotCompleted => IndustryPlacementStatusContent.NotCompleted_Display_Text,
                    _ => string.Empty,
                };
            }
        }

        public string GetEnglishAndMathsStatusDisplayText
        {
            get
            {
                return (HasLrsEnglishAndMaths, IsEnglishAndMathsAchieved, IsSendLearner == true) switch
                {
                    (true, false, false) => EnglishAndMathsStatusContent.Lrs_Not_Achieved_Display_Text,
                    (true, true, false) => EnglishAndMathsStatusContent.Lrs_Achieved_Display_Text,
                    (true, true, true) => EnglishAndMathsStatusContent.Lrs_Achieved_With_Send_Display_Text,
                    
                    (false, false, false) => EnglishAndMathsStatusContent.Not_Achieved_Display_Text,
                    (false, true, false) => EnglishAndMathsStatusContent.Achieved_Display_Text,
                    (false, true, true) => EnglishAndMathsStatusContent.Achieved_With_Send_Display_Text,
                    _ => string.Empty,
                };
            }
        }
    }
}
