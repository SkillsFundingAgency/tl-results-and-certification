using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressConfirmationGet
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ConfirmationCacheKey = string.Concat(CacheKey, Constants.AddAddressConfirmation);
            CacheService.GetAndRemoveAsync<bool>(ConfirmationCacheKey).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<bool>(ConfirmationCacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();
        }
    }
}
