using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevel
{
    public class Then_Redirected_On_Null_ViewModel : When_RemoveProviderTlevelAsync_Get_Action_Is_Called
    {
        public override void Given()
        {
            Ukprn = 10011881;
            TqProviderId = 0;
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            ProviderTlevelDetailsViewModel mockresult = null;
            ProviderLoader.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTqProviderTlevelDetailsAsync_Is_Called()
        {
            ProviderLoader.Received().GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId);
        }

        [Fact]
        public void Then_On_No_Record_Found_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
