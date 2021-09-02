using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmedTlevels
{
    public class When_Tlevels_NotFound : TestSetup
    {
        public override void Given()
        {
            var mockresult = new ConfirmedTlevelsViewModel
            {
                Tlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetConfirmedTlevelsViewModelAsync(AoUkprn)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetConfirmedTlevelsViewModelAsync(AoUkprn);
        }

        [Fact]
        public void Then_Redirected_To_NoConfirmedTlevels()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.NoConfirmedTlevels);
        }
    }
}
