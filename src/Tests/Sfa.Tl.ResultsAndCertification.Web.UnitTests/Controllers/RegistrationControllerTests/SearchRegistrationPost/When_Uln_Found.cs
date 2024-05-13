using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class When_Uln_Found : TestSetup
    {
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = SearchUln };
            var mockResult = new UlnRegistrationNotFoundViewModel { IsAllowed = true, Uln = SearchUln };
            RegistrationLoader.FindUlnAsync(AoUkprn, SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).FindUlnAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_RegistrationDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationDetails);
        }
    }
}
