using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.Profile
{
    public class When_No_ProfileUrl : TestSetup
    {
        public override void Given() 
        {
            Configuration = new Models.Configuration.ResultsAndCertificationConfiguration
            {
                DfeSignInSettings = new Models.Configuration.DfeSignInSettings()
            };
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
