using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminFindProviderGet
{
    public class When_Cache_Empty : AdminProviderControllerBaseTest
    {
        private readonly AdminFindProviderViewModel _viewModel = new()
        {
            Search = null
        };

        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminFindProviderViewModel>(CacheKey).Returns(null as AdminFindProviderViewModel);
        }

        public async override Task When()
        {
            _result = await Controller.AdminFindProviderAsync();
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminFindProviderViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var resultViewModel = _result.ShouldBeViewResult<AdminFindProviderViewModel>();
            resultViewModel.Should().BeEquivalentTo(_viewModel);
        }
    }
}