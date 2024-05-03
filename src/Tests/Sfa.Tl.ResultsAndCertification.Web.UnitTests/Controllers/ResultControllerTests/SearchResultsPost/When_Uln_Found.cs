using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.SearchResultsPost
{
    public class When_Uln_Found : TestSetup
    {
        public override void Given()
        {
            SearchResultsViewModel = new SearchResultsViewModel { SearchUln = SearchUln };
            var mockResult = new UlnResultsNotFoundViewModel { IsAllowed = true, IsWithdrawn = false, Uln = SearchUln };
            ResultLoader.FindUlnResultsAsync(AoUkprn, SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            ResultLoader.Received(1).FindUlnResultsAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_ResultWithdrawnDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ResultDetails);
        }
    }
}
