using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.ViewProviderTlevels
{
    public class When_Tlevels_NotFound : TestSetup
    {
        private ProviderViewModel mockViewmodel;

        public override void Given()
        {
            mockViewmodel = new ProviderViewModel
            {
                Tlevels = new List<TlevelViewModel>
                {
                    new TlevelViewModel { TlevelTitle = "Design: Construction" },
                    new TlevelViewModel { TlevelTitle = "Arts" },
                },
            };

            ProviderLoader.GetViewProviderTlevelViewModelAsync(Arg.Any<long>(), providerId)
                .Returns(mockViewmodel);
        }

        [Fact]
        public void Then_Redirected_To_SelectProviderTlevels()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SelectProviderTlevels);
        }
    }
}
