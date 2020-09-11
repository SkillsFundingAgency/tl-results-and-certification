using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.Profile
{
    public class When_Action_Called : TestSetup
    {
        readonly string mockUrl = "http://dfe-profile";

        public override void Given()
        {
            Configuration = new Models.Configuration.ResultsAndCertificationConfiguration
            {
                DfeSignInSettings = new Models.Configuration.DfeSignInSettings 
                {
                    ProfileUrl = mockUrl
                }
            };
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.Should().NotBeNull();
            var actualResult = Result as RedirectResult;

            actualResult.Url.Should().Be(mockUrl);
        }
    }
}
