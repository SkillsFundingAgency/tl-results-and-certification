using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminBannerControllerTests.AdminEditBannerGet
{
    public class When_BannerDetails_Not_Found : AdminBannerControllerBaseTest
    {
        private const int BannerId = 1; 

        public override void Given()
        {
            AdminBannerLoader.GetEditBannerViewModel(BannerId).Returns(null as AdminEditBannerViewModel);
        }

        private IActionResult _result;

        public async override Task When()
        {
            _result = await Controller.AdminEditBannerAsync(BannerId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminBannerLoader.GetEditBannerViewModel(BannerId);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            _result.ShouldBeRedirectToProblemWithService();
        }
    }
}