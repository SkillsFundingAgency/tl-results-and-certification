using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using System.Collections.Generic;
using Xunit;
using ContentAlreadyRequested = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaSubmittedAlready;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaAlreadySubmitted
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private RequestSoaSubmittedAlreadyViewModel _mockLearnerDetails;
        private Address _address;
        private SoaPrintingDetails _soaPrintDetails;

        public override void Given()
        {
            ProfileId = 11;
            ResultsAndCertificationConfiguration.SoaRerequestInDays = 21;
            _address = new Address { AddressId = 1, DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };
            _soaPrintDetails = new SoaPrintingDetails
            {
                Uln = 1234567890,
                Name = "John Smith",
                Dateofbirth = "09 July 2021",
                ProviderName = "Barsley College (569874567)",

                TlevelTitle = "Design, Surveying and Planning for Construction",
                Core = "Design, Surveying and Planning for Construction (60358300)",
                CoreGrade = "A*",
                Specialism = "Building Services Design (ZTLOS003)",
                SpecialismGrade = "None",

                EnglishAndMaths = "Achieved minimum standard",
                IndustryPlacement = "Placement completed",

                ProviderAddress = _address
            };

            _mockLearnerDetails = new RequestSoaSubmittedAlreadyViewModel
            {
                PathwayStatus = RegistrationPathwayStatus.Withdrawn,
                RequestedOn = DateTime.Today,
                RequestedBy = "John Smith",
                SnapshotDetails = _soaPrintDetails
            };

            StatementOfAchievementLoader.GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaSubmittedAlreadyViewModel;

            model.Should().NotBeNull();
            model.PathwayStatus.Should().Be(_mockLearnerDetails.PathwayStatus);
            model.RequestedBy.Should().Be(_mockLearnerDetails.RequestedBy);
            model.RequestedOn.Should().Be(_mockLearnerDetails.RequestedOn);
            model.RequestedDate.Should().Be(_mockLearnerDetails.RequestedOn.ToDobFormat());

            // Snapshot
            model.SnapshotDetails.Should().NotBeNull();
            model.SnapshotDetails.Uln.Should().Be(_soaPrintDetails.Uln);
            model.SnapshotDetails.Name.Should().Be(_soaPrintDetails.Name);
            model.SnapshotDetails.Dateofbirth.Should().Be(_soaPrintDetails.Dateofbirth);
            model.SnapshotDetails.ProviderName.Should().Be(_soaPrintDetails.ProviderName);
            model.SnapshotDetails.TlevelTitle.Should().Be(_soaPrintDetails.TlevelTitle);
            model.SnapshotDetails.Core.Should().Be(_soaPrintDetails.Core);
            model.SnapshotDetails.CoreGrade.Should().Be(_soaPrintDetails.CoreGrade);
            model.SnapshotDetails.Specialism.Should().Be(_soaPrintDetails.Specialism);
            model.SnapshotDetails.SpecialismGrade.Should().Be(_soaPrintDetails.SpecialismGrade);
            model.SnapshotDetails.EnglishAndMaths.Should().Be(_soaPrintDetails.EnglishAndMaths);
            model.SnapshotDetails.IndustryPlacement.Should().Be(_soaPrintDetails.IndustryPlacement);

            // Uln
            model.SummaryUln.Title.Should().Be(ContentAlreadyRequested.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_soaPrintDetails.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(ContentAlreadyRequested.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_soaPrintDetails.Name);

            // DateofBirth
            model.SummaryDateofBirth.Title.Should().Be(ContentAlreadyRequested.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(_soaPrintDetails.Dateofbirth);

            // ProviderName
            model.SummaryProvider.Title.Should().Be(ContentAlreadyRequested.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_soaPrintDetails.ProviderName);

            // TLevelTitle
            model.SummaryTlevelTitle.Title.Should().Be(ContentAlreadyRequested.Title_Tlevel_Title_Text);
            model.SummaryTlevelTitle.Value.Should().Be(_soaPrintDetails.TlevelTitle);

            // CoreCode
            model.SummaryCoreCode.Title.Should().Be(ContentAlreadyRequested.Title_Core_Code_Text);
            model.SummaryCoreCode.Value.Should().Be(string.Format(ContentAlreadyRequested.Core_Code_Value, _soaPrintDetails.Core, _soaPrintDetails.CoreGrade));

            // SpecialismCode
            model.SummarySpecialismCode.Title.Should().Be(ContentAlreadyRequested.Title_Occupational_Specialism_Text);
            model.SummarySpecialismCode.Value.Should().Be(string.Format(ContentAlreadyRequested.Occupational_Specialism_Value, _soaPrintDetails.Specialism, _soaPrintDetails.SpecialismGrade));

            // EnglishAndMaths
            model.SummaryEnglishAndMaths.Title.Should().Be(ContentAlreadyRequested.Title_English_And_Maths_Text);
            model.SummaryEnglishAndMaths.Value.Should().Be(_soaPrintDetails.EnglishAndMaths);

            // Industry Placement
            model.SummaryIndustryPlacement.Title.Should().Be(ContentAlreadyRequested.Title_Industry_Placement_Text);
            model.SummaryIndustryPlacement.Value.Should().Be(_soaPrintDetails.IndustryPlacement);

            // Department
            model.SummaryDepartment.Title.Should().Be(ContentAlreadyRequested.Title_Department_Text);
            model.SummaryDepartment.Value.Should().Be(_address.DepartmentName);

            // Address
            model.SummaryAddress.Title.Should().Be(ContentAlreadyRequested.Title_Organisation_Address_Text);
            model.SummaryAddress.Value.Should().Be(string.Format(ContentAlreadyRequested.Organisation_Address_Value, FormatedAddress));

            // Breadcrum 
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RequestStatementOfAchievement);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Learner);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.StatementOfAchievementRequested);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNull();
        }

        private string FormatedAddress
        {
            get
            {
                var addressLines = new List<string> { _address.OrganisationName, _address.AddressLine1, _address.AddressLine2, _address.Town, _address.Postcode };
                return string.Join(ContentAlreadyRequested.Html_Line_Break, addressLines.Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
    }
}