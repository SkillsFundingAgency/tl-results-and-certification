using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsGet
{
    public class Then_Return_ProviderTlevels_BackLink_In_ViewModel : When_SelectProviderTlevelsAsync_Get_Action_Is_Called
    {
        private ProviderTlevelsViewModel mockresult;
        private int _providerId = 1;

        public override void Given()
        {
            mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = _providerId,
                DisplayName = "Test",
                Ukprn = 10000111,
                IsAddTlevel = true,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = _providerId, TlevelTitle = "Route1: Pathway1" },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = _providerId, TlevelTitle = "Route1: Pathway1" }
                }
            };

            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(mockresult);
        }

        public override void When()
        {
            Result = Controller.AddProviderTlevelsAsync(ProviderId);
        }

        [Fact]
        public void Then_Returns_ProviderTlevels_BackLink()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ProviderTlevelsViewModel;

            model.Should().NotBeNull();
            var backLink = model.BackLink;

            backLink.RouteName.Should().Be(RouteConstants.ProviderTlevels);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue("providerId", out string routeValue);
            routeValue.Should().Be(_providerId.ToString());
        }
    }
}
