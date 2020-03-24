using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderGet
{
    public class Then_FindProviderViewModel_Is_Returned : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_Expeced_Results_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));
            
            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Should().NotBeNull();
            model.Search.Should().BeNull();
            model.SelectedProviderId.Should().Be(0);
        }
    }
}
