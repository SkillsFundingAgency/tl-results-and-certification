using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsGet
{
    public class When_IsAddTlevel_IsFalse : TestSetup
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

        public async override Task When()
        {
            Result = await Controller.AddProviderTlevelsAsync(ProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
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
