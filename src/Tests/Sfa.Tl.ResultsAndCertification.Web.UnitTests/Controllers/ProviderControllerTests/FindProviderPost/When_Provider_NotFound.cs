using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class When_Provider_NotFound : TestSetup
    {
        private IEnumerable<ProviderLookupData> expectedMockProviders;

        public override void Given()
        {
            ViewModel.SelectedProviderId = 0;

            expectedMockProviders = new List<ProviderLookupData>();
            ProviderLoader.GetProviderLookupDataAsync(ViewModel.Search, true)
                .Returns(expectedMockProviders);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            ProviderLoader.Received(1).GetProviderLookupDataAsync(ViewModel.Search, true);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(FindProviderViewModel));

            var model = viewResult.Model as FindProviderViewModel;
            model.Search.Should().Be(ProviderName);
            model.SelectedProviderId.Should().Be(0);

            // Assert Error
            Controller.ModelState.IsValid.Should().Be(false);
            Controller.ModelState.ErrorCount.Should().Be(1);

            var expectedErrorMessage = Controller.ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage;
            expectedErrorMessage.Should().Be(Content.Provider.FindProvider.ProviderName_NotValid_Validation_Message);
        }
    }
}
