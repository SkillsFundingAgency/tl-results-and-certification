using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeProviderNotOfferingSameCoreGet
{
    public class When_Cache_Found : TestSetup
    {
        private ChangeProviderCoreNotSupportedViewModel cacheResult;

        public override void Given()
        {
            cacheResult = new ChangeProviderCoreNotSupportedViewModel
            {
                ProfileId = 1,
                ProviderDisplayName = "Test (12345678)",
                CoreDisplayName = "Test core (987654321)"
            };

            CacheService.GetAsync<ChangeProviderCoreNotSupportedViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<ChangeProviderCoreNotSupportedViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();

            var actualModel = viewResult.Model as ChangeProviderNotOfferingSameCoreViewModel;
            actualModel.ProfileId.Should().Be(cacheResult.ProfileId);
            actualModel.ProviderDisplayName.Should().Be(cacheResult.ProviderDisplayName);
            actualModel.CoreDisplayName.Should().Be(cacheResult.CoreDisplayName);
        }
    }
}
