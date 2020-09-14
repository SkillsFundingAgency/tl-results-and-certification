using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new FindProviderViewModel();
            Controller.ModelState.AddModelError("Search", "Please enter the provider name");
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Search.Should().BeNull();
            model.SelectedProviderId.Should().Be(0);
        }
    }
}
