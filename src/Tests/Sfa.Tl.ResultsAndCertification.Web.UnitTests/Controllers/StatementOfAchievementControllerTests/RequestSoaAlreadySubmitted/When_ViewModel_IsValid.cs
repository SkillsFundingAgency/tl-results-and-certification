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
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ContentAlreadyRequested = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaAlreadySubmitted;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaAlreadySubmitted
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private RequestSoaAlreadySubmittedViewModel _mockLearnerDetails;
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

                IndustryPlacement = "Placement completed",

                ProviderAddress = _address
            };

            _mockLearnerDetails = new RequestSoaAlreadySubmittedViewModel
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
            var model = viewResult.Model as RequestSoaAlreadySubmittedViewModel;

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
            model.SnapshotDetails.IndustryPlacement.Should().Be(_soaPrintDetails.IndustryPlacement);

            // Uln
            model.SummaryUln.Title.Should().Be(ContentAlreadyRequested.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_soaPrintDetails.Uln.ToString());

            // LearnerName
            model.SummaryLearnerName.Title.Should().Be(ContentAlreadyRequested.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(_soaPrintDetails.Name);

            // Date requested
            model.SummaryDateRequested.Title.Should().Be(ContentAlreadyRequested.Title_DateRequested_Text);
            model.SummaryDateRequested.Value.Should().Be(_mockLearnerDetails.RequestedDate);

            // Requested by
            model.SummaryRequestedBy.Title.Should().Be(ContentAlreadyRequested.Title_RequestedBy_Text);
            model.SummaryRequestedBy.Value.Should().Be(_mockLearnerDetails.RequestedBy);

            // Breadcrum 
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);

            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Request_Statement_Of_Achievement);
        }
    }
}