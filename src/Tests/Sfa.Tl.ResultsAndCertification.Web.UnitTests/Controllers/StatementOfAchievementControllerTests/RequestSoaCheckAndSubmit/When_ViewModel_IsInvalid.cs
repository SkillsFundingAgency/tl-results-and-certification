using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmit
{
    public class When_ViewModel_IsInvalid : TestSetup
    {
        private FindSoaLearnerRecord _mockFindSoaLearnerRecord = null;
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            _mockFindSoaLearnerRecord = new FindSoaLearnerRecord { ProfileId = 11 };
            CacheService.GetAsync<FindSoaLearnerRecord>(CacheKey).Returns(_mockFindSoaLearnerRecord);

            _mockLearnerDetails = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = _mockFindSoaLearnerRecord.ProfileId,
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

                HasPathwayResult  = false,
                IsNotWithdrawn = false,
                IsLearnerRegistered = true,
                IsIndustryPlacementAdded = true,
                IsIndustryPlacementCompleted = false,

                ProviderAddress = new AddressViewModel { DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" },
            };

            StatementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, _mockFindSoaLearnerRecord.ProfileId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<FindSoaLearnerRecord>(CacheKey);
            StatementOfAchievementLoader.Received(1).GetSoaLearnerRecordDetailsAsync(ProviderUkprn, _mockFindSoaLearnerRecord.ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
