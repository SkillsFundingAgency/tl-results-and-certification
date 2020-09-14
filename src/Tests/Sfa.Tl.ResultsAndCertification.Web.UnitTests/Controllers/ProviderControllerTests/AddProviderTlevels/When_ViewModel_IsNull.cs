using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.AddProviderTlevels
{
    public class When_ViewModel_IsNull : TestSetup
    {
        public override void Given()
        {
            Ukprn = 10011881;
            ProviderId = 1;
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            ProviderTlevelsViewModel mockresult = null;
            ProviderLoader.GetSelectProviderTlevelsAsync(Arg.Any<long>(), Arg.Any<int>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            ProviderLoader.Received().GetSelectProviderTlevelsAsync(Ukprn, ProviderId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
