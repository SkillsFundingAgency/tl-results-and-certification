using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmitPost
{
    public class When_SoaDetails_IsInvalid : TestSetup
    {
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            SoaLearnerRecordDetailsViewModel = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderName = "Barsley College",

                TlevelTitle = "Design, Surveying and Planning for Construction",
                PathwayName = "Design, Surveying and Planning for Construction(60358300)",
                PathwayGrade = "A*",
                SpecialismName = "Building Services Design (ZTLOS003)",
                SpecialismGrade = "None",

                IsEnglishAndMathsAchieved = true,
                HasLrsEnglishAndMaths = false,
                IsSendLearner = true,
                IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,

                HasPathwayResult = false,
                IsNotWithdrawn = false,
                IsLearnerRegistered = true,
                IsIndustryPlacementAdded = true,
                IsIndustryPlacementCompleted = false,

                ProviderAddress = new AddressViewModel { DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" },
            };

            SoaLearnerRecordDetailsViewModel = new SoaLearnerRecordDetailsViewModel { ProfileId = ProfileId };
            StatementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
