using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsNotFound
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<UlnAssessmentsNotFoundViewModel>(Arg.Any<string>()).Returns(new UlnAssessmentsNotFoundViewModel { Uln = Uln });
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<UlnAssessmentsNotFoundViewModel>(string.Concat(CacheKey, Constants.SearchAssessmentsUlnNotFound));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UlnAssessmentsNotFoundViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(Uln);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchAssessments);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }
    }
}
