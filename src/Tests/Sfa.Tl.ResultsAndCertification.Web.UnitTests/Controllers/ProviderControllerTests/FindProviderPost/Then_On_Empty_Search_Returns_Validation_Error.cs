using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class Then_On_Empty_Search_Returns_Validation_Error : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ViewModel = new FindProviderViewModel();
            Controller.ModelState.AddModelError("Search", "Please enter the provider name");
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
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
