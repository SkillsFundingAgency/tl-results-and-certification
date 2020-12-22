using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsPost
{
    public class When_Uln_NotFound : TestSetup
    {
        public override void Given()
        {
            SearchAssessmentsViewModel = new SearchAssessmentsViewModel { SearchUln = SearchUln };
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).FindUlnAssessmentsAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_SearchAssessmentsNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SearchAssessmentsNotFound);
        }
    }
}
