using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.FindProviderPost
{
    public class Then_On_SelectedProviderId_Zero_ProviderFound_Returns_Expected_Results : When_FindProviderAsync_Post_Action_Is_Called
    {
        private IEnumerable<ProviderLookupData> expectedMockProviders;

        public override void Given()
        {
            ViewModel.SelectedProviderId = 0;

            expectedMockProviders = new List<ProviderLookupData>
            {
                new ProviderLookupData { Id = 1, DisplayName = "Kings Edward Sixth Form" },
            };
            ProviderLoader.GetProviderLookupDataAsync(ViewModel.Search, true)
                .Returns(expectedMockProviders);
        }

        [Fact]
        public void Then_Redirected_ToSelectProviderTLevels()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SelectProviderTlevels);
        }
    }
}
