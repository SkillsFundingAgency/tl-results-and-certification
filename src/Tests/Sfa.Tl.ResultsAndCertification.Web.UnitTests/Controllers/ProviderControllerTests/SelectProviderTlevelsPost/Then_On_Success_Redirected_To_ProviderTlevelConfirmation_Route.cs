using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsPost
{
    public class Then_On_Success_Redirected_To_ProviderTlevelConfirmation_Route : When_SelectProviderTlevelsAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            InputViewModel = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1", IsSelected = true },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" }
                }
            };

            ProviderLoader.AddProviderTlevelsAsync(InputViewModel).Returns(true);
        }

        [Fact]
        public void Then_ModelState_Valid_Redirected_To_ProviderTlevelConfirmation()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProviderTlevelConfirmation);
        }
    }
}
