using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestSoaCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class SoaLearnerRecordDetailsViewModel
    {
        //Learner's registration details
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }

        //Learner's technical qualification details
        public string TlevelTitle { get; set; }
        public string PathwayName { get; set; }
        public string PathwayGrade { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismGrade { get; set; }

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
            Value = ProviderName
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

    }
}
