using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using EnglishAndMathsStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.EnglishAndMathsStatus;
using IndustryPlacementStatusContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetSoaLearnerRecordDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private SoaLearnerRecordDetails _expectedApiResult;
        private Address _address;

        public override void Given()
        {
            _address = new Address { DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };

            _expectedApiResult = new SoaLearnerRecordDetails
            {
                ProfileId = 99,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderName = "Barsley College",

                TlevelTitle = "Design, Surveying and Planning for Construction",
                PathwayName = "Design, Surveying and Planning for Construction(60358300)",
                PathwayGrade = "A*",
                SpecialismName = "Building Services Design (ZTLOS003)",
                SpecialismGrade = "None",

                IsEnglishAndMathsAchieved = false,
                HasLrsEnglishAndMaths = true,
                IsSendLearner = false,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,

                ProviderAddress = _address,
                Status = RegistrationPathwayStatus.Withdrawn,
            };

            InternalApiClient.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {

            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.LearnerName);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);

            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.PathwayName.Should().Be(_expectedApiResult.PathwayName);
            ActualResult.PathwayGrade.Should().Be(_expectedApiResult.PathwayGrade);
            ActualResult.SpecialismName.Should().Be(_expectedApiResult.SpecialismName);
            ActualResult.SpecialismGrade.Should().Be(_expectedApiResult.SpecialismGrade);

            ActualResult.IsEnglishAndMathsAchieved.Should().Be(_expectedApiResult.IsEnglishAndMathsAchieved);
            ActualResult.HasLrsEnglishAndMaths.Should().Be(_expectedApiResult.HasLrsEnglishAndMaths);
            ActualResult.IsSendLearner.Should().Be(_expectedApiResult.IsSendLearner);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);

            // Uln
            ActualResult.SummaryUln.Title.Should().Be(CheckAndSubmitContent.Title_Uln_Text);
            ActualResult.SummaryUln.Value.Should().Be(_expectedApiResult.Uln.ToString());

            // LearnerName
            ActualResult.SummaryLearnerName.Title.Should().Be(CheckAndSubmitContent.Title_Name_Text);
            ActualResult.SummaryLearnerName.Value.Should().Be(_expectedApiResult.LearnerName);

            // DateofBirth
            ActualResult.SummaryDateofBirth.Title.Should().Be(CheckAndSubmitContent.Title_DateofBirth_Text);
            ActualResult.SummaryDateofBirth.Value.Should().Be(_expectedApiResult.DateofBirth.ToDobFormat());

            // ProviderName
            ActualResult.SummaryProvider.Title.Should().Be(CheckAndSubmitContent.Title_Provider_Text);
            ActualResult.SummaryProvider.Value.Should().Be(_expectedApiResult.ProviderName);

            // TLevelTitle
            ActualResult.SummaryTlevelTitle.Title.Should().Be(CheckAndSubmitContent.Title_Tlevel_Title_Text);
            ActualResult.SummaryTlevelTitle.Value.Should().Be(_expectedApiResult.TlevelTitle);

            // CoreCode
            ActualResult.SummaryCoreCode.Title.Should().Be(CheckAndSubmitContent.Title_Core_Code_Text);
            ActualResult.SummaryCoreCode.Value.Should().Be(string.Format(CheckAndSubmitContent.Core_Code_Value, _expectedApiResult.PathwayName, _expectedApiResult.PathwayGrade));

            // SpecialismCode
            ActualResult.SummarySpecialismCode.Title.Should().Be(CheckAndSubmitContent.Title_Occupational_Specialism_Text);
            ActualResult.SummarySpecialismCode.Value.Should().Be(string.Format(CheckAndSubmitContent.Occupational_Specialism_Value, _expectedApiResult.SpecialismName, _expectedApiResult.SpecialismGrade));

            // EnglishAndMaths
            ActualResult.SummaryEnglishAndMaths.Title.Should().Be(CheckAndSubmitContent.Title_English_And_Maths_Text);
            ActualResult.SummaryEnglishAndMaths.Value.Should().Be(EnglishAndMathsStatusContent.Lrs_Not_Achieved_Display_Text);

            // Industry Placement
            ActualResult.SummaryIndustryPlacement.Title.Should().Be(CheckAndSubmitContent.Title_Industry_Placement_Text);
            ActualResult.SummaryIndustryPlacement.Value.Should().Be(IndustryPlacementStatusContent.CompletedWithSpecialConsideration_Display_Text);

            // Department
            ActualResult.SummaryDepartment.Title.Should().Be(CheckAndSubmitContent.Title_Department_Text);
            ActualResult.SummaryDepartment.Value.Should().Be(_address.DepartmentName);

            // Address
            ActualResult.SummaryAddress.Title.Should().Be(CheckAndSubmitContent.Title_Organisation_Address_Text);
            ActualResult.SummaryAddress.Value.Should().Be(string.Format(CheckAndSubmitContent.Organisation_Address_Value, _formatedAddress));

            // validation properties
            ActualResult.HasPathwayResult.Should().BeTrue();
            ActualResult.IsIndustryPlacementAdded.Should().BeTrue();
            ActualResult.IsLearnerRegistered.Should().BeTrue();
            ActualResult.IsNotWithdrawn.Should().BeFalse();
            ActualResult.IsIndustryPlacementCompleted.Should().BeTrue();

            // Breadcrum 
            ActualResult.Breadcrumb.Should().NotBeNull();
            ActualResult.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            ActualResult.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            ActualResult.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            ActualResult.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
            ActualResult.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RequestStatementOfAchievement);
            ActualResult.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            ActualResult.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            ActualResult.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Check_Learner_Details);
            ActualResult.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNull();
        }

        private string _formatedAddress
        {
            get
            {
                var addressLines = new List<string> { _address.OrganisationName, _address.AddressLine1, _address.AddressLine2, _address.Town, _address.Postcode };
                return string.Join(CheckAndSubmitContent.Html_Line_Break, addressLines.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
    }
}
