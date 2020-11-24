using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.SearchAssessmentsPost
{    
    public class When_Uln_Withdrawn_Found : TestSetup
    {
        public override void Given()
        {
            SearchAssessmentsViewModel = new SearchAssessmentsViewModel { SearchUln = SearchUln };
            var mockResult = new UlnAssessmentsNotFoundViewModel { IsAllowed = true, IsWithdrawn = true, Uln = SearchUln };
            AssessmentLoader.FindUlnAssessmentsAsync(AoUkprn, SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).FindUlnAssessmentsAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_AssessmentWithdrawnDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AssessmentWithdrawnDetails);
        }
    }
}
