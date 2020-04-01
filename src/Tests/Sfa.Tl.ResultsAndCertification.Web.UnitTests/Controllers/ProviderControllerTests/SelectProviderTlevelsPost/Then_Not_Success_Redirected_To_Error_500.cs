using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsPost
{
    public class Then_Not_Success_Redirected_To_Error_500 : When_SelectProviderTlevelsAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            InputViewModel = InputViewModel = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, PathwayId = 1, TlevelTitle = "Route1: Pathway1", IsSelected = false },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, PathwayId = 2, TlevelTitle = "Route1: Pathway1", IsSelected = false }
                }
            };
            ProviderLoader.AddProviderTlevelsAsync(InputViewModel).Returns(false);
        }

        [Fact]
        public void Then_Status_Update_Fail_Redirected_To_Error_500()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be("error/500");
        }
    }
}
