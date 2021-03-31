using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordNotAddedGet
{
    public class When_IsLearnerRecordAdded_Already : TestSetup
    {
        private readonly long uln = 1234567890;
        private SearchLearnerRecordViewModel cacheModel = null;

        public override void Given()
        {
            cacheModel = new SearchLearnerRecordViewModel { SearchUln = uln.ToString(), IsLearnerRecordAdded = true };
            CacheService.GetAsync<SearchLearnerRecordViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<SearchLearnerRecordViewModel>(CacheKey);
        }
    }
}
