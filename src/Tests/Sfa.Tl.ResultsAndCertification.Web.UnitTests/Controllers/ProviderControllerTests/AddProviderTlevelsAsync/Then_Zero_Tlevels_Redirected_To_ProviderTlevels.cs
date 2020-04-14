using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.AddProviderTlevelsAsync
{
    public class Then_Zero_Tlevels_Redirected_To_ProviderTlevels : When_AddProviderTlevelsAsync_Get_Action_Is_Called
    {
        private ProviderTlevelsViewModel mockresult;
        public override void Given()
        {
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            mockresult = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test",
                Ukprn = 10000111,
                Tlevels = new List<ProviderTlevelViewModel>()
            };

            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(mockresult);
        }

        [Fact]
        public void Then_Zero_Tlevels_Redirected_To_ProviderTlevels_Route()
        {
            var route = (Result.Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.ProviderTlevels);

            route.RouteValues["providerId"].Should().Be(ProviderId);
            route.RouteValues["navigation"].Should().Be(true);
        }
    }
}
