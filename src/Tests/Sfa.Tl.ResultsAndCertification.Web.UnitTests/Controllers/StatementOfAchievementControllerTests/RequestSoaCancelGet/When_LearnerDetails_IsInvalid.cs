using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelGet
{
    public class When_LearnerDetails_IsInvalid : TestSetup
    {
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            _mockLearnerDetails = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = ProfileId,
                LearnerName = "John Smith",
                
                IsLearnerRegistered = false,
                IsNotWithdrawn = true,
                IsIndustryPlacementAdded = false,
            };

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
