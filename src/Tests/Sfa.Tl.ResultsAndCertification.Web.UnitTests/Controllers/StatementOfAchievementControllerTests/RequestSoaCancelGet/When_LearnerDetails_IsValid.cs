using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelGet
{
    public class When_LearnerDetails_IsValid : TestSetup
    {
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            _mockLearnerDetails = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                LearnerName = "John Smith",

                IsEnglishAndMathsAchieved = true,
                HasLrsEnglishAndMaths = false,
                IsSendLearner = true,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed,

                HasPathwayResult = true,
                IsNotWithdrawn = false,
                IsLearnerRegistered = true,
                IsIndustryPlacementAdded = true,
                IsIndustryPlacementCompleted = true,
            };

            StatementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_mockLearnerDetails);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as RequestSoaCancelViewModel;
            model.ProfileId.Should().Be(_mockLearnerDetails.ProfileId);
            model.LearnerName.Should().Be(_mockLearnerDetails.LearnerName);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_mockLearnerDetails.ProfileId.ToString());
        }
    }
}