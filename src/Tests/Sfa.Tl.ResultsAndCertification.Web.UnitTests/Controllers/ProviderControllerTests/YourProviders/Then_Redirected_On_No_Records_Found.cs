using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.YourProviders
{
    public class Then_Redirected_On_No_Records_Found :  When_YourProvidersAsync_Get_Action_Is_Called
    {
        public override void Given()
        {
            Ukprn = 10011881;
            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            var mockresult = new List<ProviderDetailsViewModel>();
            ProviderLoader.GetTqAoProviderDetailsAsync(Ukprn)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_Is_Called()
        {
            ProviderLoader.Received().GetTqAoProviderDetailsAsync(Ukprn);
        }

        [Fact]
        public void Then_On_No_Record_Found_Redirect_To_FindProiver()
        {
            var actualRouteName = (Result.Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.FindProvider);
        }
    }
}
