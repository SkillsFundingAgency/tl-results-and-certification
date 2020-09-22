using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.WithdrawConfirmationGet
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAndRemoveAsync<WithdrawRegistrationResponse>(CacheKey).Returns(MockResult);
        }

        [Fact]
        public void Then_Returns_Expected_View()
        {
            Result.Should().NotBeNull();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var actualModel = viewResult.Model as WithdrawRegistrationResponse;
            actualModel.Uln.Should().Be(MockResult.Uln);
            actualModel.ProfileId.Should().Be(MockResult.ProfileId);
            actualModel.IsRequestFromProviderAndCorePage.Should().BeTrue();
        }
    }
}
