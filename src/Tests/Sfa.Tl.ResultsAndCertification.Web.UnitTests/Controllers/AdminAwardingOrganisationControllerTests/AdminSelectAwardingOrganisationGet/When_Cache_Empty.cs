using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminSelectAwardingOrganisationGet
{
    public class When_Cache_Empty : AdminSelectAwardingOrganisationBaseTest
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminSelectAwardingOrganisationViewModel>(CacheKey).ReturnsNull();
            Loader.GetSelectAwardingOrganisationViewModelAsync().Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminSelectAwardingOrganisationViewModel>(CacheKey);
            Loader.Received(1).GetSelectAwardingOrganisationViewModelAsync();

            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminSelectAwardingOrganisationViewModel>();
            model.Should().BeEquivalentTo(ViewModel);
        }
    }
}