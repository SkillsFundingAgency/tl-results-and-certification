using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregistrationConfirmationGet
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<ReregistrationResponse>(Arg.Any<string>()).Returns(MockResult);
        }

        [Fact]
        public void Then_Returns_Expected_Result()
        {
            Result.Should().NotBeNull();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var actualModel = viewResult.Model as ReregistrationResponse;
            actualModel.Uln.Should().Be(MockResult.Uln);
            actualModel.ProfileId.Should().Be(MockResult.ProfileId);
        }
    }
}
