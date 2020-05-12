using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.SelectProviderTlevelsGet
{
    public class Then_Return_FindProvider_BackLink_In_ViewModel : When_SelectProviderTlevelsAsync_Get_Action_Is_Called
    {
        private ProviderTlevelsViewModel mockresult;

        public override void Given()
        {
            mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                IsAddTlevel = false,
                Tlevels = new List<ProviderTlevelViewModel>
                {
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" },
                    new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, TlevelTitle = "Route1: Pathway1" }
                }
            };

            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(mockresult);
        }

       [Fact]
        public void Then_Returns_FindProvider_BackLink()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ProviderTlevelsViewModel;

            model.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.FindProvider);
            model.BackLink.RouteAttributes["isback"].Should().Be("true");
        }
    }
}
