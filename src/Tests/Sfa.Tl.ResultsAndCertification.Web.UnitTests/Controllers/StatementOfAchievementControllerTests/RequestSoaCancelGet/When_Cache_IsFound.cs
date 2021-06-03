using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private FindSoaLearnerRecord _mockCache;

        public override void Given()
        {
            _mockCache = new FindSoaLearnerRecord { ProfileId = 1, LearnerName = "John Smith" };
            CacheService.GetAsync<FindSoaLearnerRecord>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAsync<FindSoaLearnerRecord>(CacheKey);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as RequestSoaCancelViewModel;
            model.ProfileId.Should().Be(_mockCache.ProfileId);
            model.LearnerName.Should().Be(_mockCache.LearnerName);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_mockCache.ProfileId.ToString());
        }
    }
}
