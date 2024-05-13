using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.SearchRegistrationPost
{
    public class When_Uln_NotFound : TestSetup
    {
        public override void Given()
        {
            SearchRegistrationViewModel = new SearchRegistrationViewModel { SearchUln = SearchUln };
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).FindUlnAsync(AoUkprn, SearchUln.ToLong());
        }

        [Fact]
        public void Then_Redirected_To_SearchRegistrationNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SearchRegistrationNotFound);
        }
    }
}
