using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelGet
{
    public class Then_Return_ProviderTlevels_BackLink_In_ViewModel : When_RemoveProviderTlevelAsync_Get_Action_Is_Called
    {
        private int _tlProviderId = 1;
        private ProviderTlevelDetailsViewModel mockresult;
        public override void Given()
        {
            Ukprn = 10011881;
            TqProviderId = 1;

            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            mockresult = new ProviderTlevelDetailsViewModel
            {
                Id = 1,
                TlProviderId = _tlProviderId,
                DisplayName = "Test",
                Ukprn = 10000111,
                TlevelTitle = "Test Title"
            };

            ProviderLoader.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_ProviderTlevels_BackLink()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ProviderTlevelDetailsViewModel;

            model.Should().NotBeNull();
            var backLink = model.BackLink;

            backLink.RouteName.Should().Be(RouteConstants.ProviderTlevels);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue("providerId", out string routeValue);
            routeValue.Should().Be(_tlProviderId.ToString());
        }
    }
}
