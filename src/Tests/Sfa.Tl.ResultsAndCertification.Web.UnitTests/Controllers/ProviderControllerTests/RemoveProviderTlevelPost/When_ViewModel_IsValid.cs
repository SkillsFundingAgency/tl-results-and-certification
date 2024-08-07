﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        public override void Given()
        {
            Ukprn = 1000001;
            TqProviderId = 1;
            TlProviderId = 5;

            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
                .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
                .Build()
                .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);

            ViewModel = new ProviderTlevelDetailsViewModel { Id = TqProviderId, TlProviderId = TlProviderId, CanRemoveTlevel = true, ShowBackToProvidersLink = true };
            ProviderLoader.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId).Returns(true);
        }

        [Fact]
        public void Then_GetTqAoProviderDetailsAsync_Method_Is_Called()
        {
            ProviderLoader.Received(1).GetTqAoProviderDetailsAsync(Ukprn);
        }

        [Fact]
        public void Then_Redirected_To_RemoveProviderTlevelConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RemoveProviderTlevelConfirmation);
        }
    }
}
