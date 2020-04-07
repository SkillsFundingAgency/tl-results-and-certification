using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AccountControllerTests.Profile
{
    public class Then_Expected_Results_Are_Returned : When_PostSignIn_Is_Called
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
