using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminProviderControllerTests.AdminFindProviderGet
{
    public class When_Cache_Not_Empty : AdminProviderControllerBaseTest
    {
        private readonly AdminFindProviderViewModel _viewModel = new()
        {
            Search = "Barnsley College"
        };

        private IActionResult _result;

        public override void Given()
        {
            CacheService.GetAsync<AdminFindProviderViewModel>(CacheKey).Returns(_viewModel);
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
            resultViewModel.Should().Be(_viewModel);
        }
    }
}