using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsPost
{
    public class Then_On_Success_Redirected_To_ProviderTlevelConfirmation_Route : When_SelectProviderTlevelsAsync_Post_Action_Is_Called
    {
        private readonly int pathwayId = 99;

        public override void Given()
        {
            InputViewModel = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelDetailsViewModel>
                {
                    new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, PathwayId = 1, TlevelTitle = "Route1: Pathway1", IsSelected = true },
                    new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, PathwayId = 2, TlevelTitle = "Route1: Pathway1" }
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
