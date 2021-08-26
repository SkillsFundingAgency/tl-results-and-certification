using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.NoConfirmedTlevels
{
    public class When_Confirmed_Tlevels_Found : TestSetup
    {
        public override void Given()
        {
            var mockresult = new ConfirmedTlevelsViewModel
            {
                Tlevels = new List<YourTlevelViewModel> { new YourTlevelViewModel { PathwayId = 1, TlevelTitle = "Tlevel in Education" } }
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
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
