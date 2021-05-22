using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUlnNotFound
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private RequestSoaUlnNotFoundViewModel mockCache = null;

        public override void Given()
        {
            mockCache = new RequestSoaUlnNotFoundViewModel { Uln = uln.ToString() };
            CacheService.GetAsync<RequestSoaUlnNotFoundViewModel>(CacheKey)
                .Returns(mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestSoaUlnNotFoundViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(uln.ToString());

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaUniqueLearnerNumber);
            model.BackLink.RouteAttributes.Should().BeEmpty();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<RequestSoaUlnNotFoundViewModel>(CacheKey);
        }
    }
}
