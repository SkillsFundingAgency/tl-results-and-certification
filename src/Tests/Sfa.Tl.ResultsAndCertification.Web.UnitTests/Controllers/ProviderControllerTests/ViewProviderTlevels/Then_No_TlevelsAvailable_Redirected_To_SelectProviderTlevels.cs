using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.ViewProviderTlevels
{
    public class Then_No_TlevelsAvailable_Redirected_To_SelectProviderTlevels : When_ViewProviderTlevelsAsync_Is_Called
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
        public void Then_Redirected_To_SelecctProviderTlevels()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.SelectProviderTlevels);
        }
    }
}
