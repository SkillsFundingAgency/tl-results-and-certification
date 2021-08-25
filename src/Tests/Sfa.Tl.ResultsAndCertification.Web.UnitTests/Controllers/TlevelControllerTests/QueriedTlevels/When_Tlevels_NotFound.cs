using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.QueriedTlevels
{
    public class When_Tlevels_NotFound : TestSetup
    {
        public override void Given()
        {
            var mockresult = new QueriedTlevelsViewModel
            {
                Tlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetQueriedTlevelsViewModelAsync(AoUkprn)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetQueriedTlevelsViewModelAsync(AoUkprn);
        }

        [Fact(Skip = "New Page Next story")]
        public void Then_Redirected_To_TODO()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.Home);
        }
    }
}
