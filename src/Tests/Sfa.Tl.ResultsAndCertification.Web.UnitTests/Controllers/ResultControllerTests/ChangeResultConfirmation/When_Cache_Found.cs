using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeResultConfirmation
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ResultConfirmationViewModel { ProfileId = 1, Uln = 1234567890 };
            CacheService.GetAndRemoveAsync<ResultConfirmationViewModel>(string.Concat(CacheKey, Constants.ChangeResultConfirmationViewModel)).Returns(ViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as ResultConfirmationViewModel;

            model.Should().NotBeNull();
            model.Uln.Should().Be(ViewModel.Uln);
            model.ProfileId.Should().Be(ViewModel.ProfileId);
        }
    }
}
