using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmitPost
{
    public class When_Failed : TestSetup
    {
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;
        private AddressViewModel _address;

        public override void Given()
        {
            ProfileId = 11;
            _address = new AddressViewModel { AddressId = 1, DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };
            _mockLearnerDetails = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderDisplayName = "Barsley College (569874567)",
                ProviderName = "Barsley College",
                ProviderUkprn = 569874567,

                TlevelTitle = "Design, Surveying and Planning for Construction",
                RegistrationPathwayId = 1,
                PathwayDisplayName = "Design, Surveying and Planning for Construction (60358300)",
                PathwayName = "Design, Surveying and Planning for Construction",
                PathwayCode = "60358300",
                PathwayGrade = "A*",
                SpecialismDisplayName = "Building Services Design (ZTLOS003)",
                SpecialismName = "Building Services Design",
                SpecialismCode = "ZTLOS003",
                SpecialismGrade = "None",

                IsEnglishAndMathsAchieved = true,
                HasLrsEnglishAndMaths = false,
                IsSendLearner = true,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,

                HasPathwayResult = true,
                IsNotWithdrawn = false,
                IsLearnerRegistered = true,
                IsIndustryPlacementAdded = true,
                IsIndustryPlacementCompleted = true,

                ProviderAddress = _address,
            };

            SoaLearnerRecordDetailsViewModel = new SoaLearnerRecordDetailsViewModel { ProfileId = ProfileId };
            SoaPrintingResponse = new SoaPrintingResponse { IsSuccess = false };

            StatementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_mockLearnerDetails);
            StatementOfAchievementLoader.CreateSoaPrintingRequestAsync(ProviderUkprn, _mockLearnerDetails).Returns(SoaPrintingResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            StatementOfAchievementLoader.Received(1).CreateSoaPrintingRequestAsync(ProviderUkprn, _mockLearnerDetails);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            var routeValue = (Result as RedirectToRouteResult).RouteValues["StatusCode"];
            routeName.Should().Be(RouteConstants.Error);
            routeValue.Should().Be(500);
        }
    }
}
