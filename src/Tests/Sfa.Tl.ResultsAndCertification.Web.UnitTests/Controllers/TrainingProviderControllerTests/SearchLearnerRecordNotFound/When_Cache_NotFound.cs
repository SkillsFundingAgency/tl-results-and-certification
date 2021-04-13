using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordNotFound
{
    public class When_Cache_NotFound : TestSetup
    {
        private readonly SearchLearnerRecordViewModel mockCache = null;
        public override void Given()
        {
            CacheService.GetAsync<SearchLearnerRecordViewModel>(CacheKey).Returns(mockCache);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAsync<SearchLearnerRecordViewModel>(CacheKey);
        }
    }
}
