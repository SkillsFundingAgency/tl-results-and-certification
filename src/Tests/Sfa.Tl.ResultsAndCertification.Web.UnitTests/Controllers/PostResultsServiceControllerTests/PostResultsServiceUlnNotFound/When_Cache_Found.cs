using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PostResultsServiceUlnNotFound
{
    public class When_Cache_Found : TestSetup
    {
        private readonly long uln = 1234567890;
        private PostResultServiceUlnNotFoundViewModel _mockCache = null;

        public override void Given()
        {
            _mockCache = new PostResultServiceUlnNotFoundViewModel { Uln = uln.ToString() };
            CacheService.GetAndRemoveAsync<PostResultServiceUlnNotFoundViewModel>(CacheKey).Returns(_mockCache);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PostResultServiceUlnNotFoundViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(uln.ToString());

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchPostResultsService);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<PostResultServiceUlnNotFoundViewModel>(CacheKey);
        }
    }
}
