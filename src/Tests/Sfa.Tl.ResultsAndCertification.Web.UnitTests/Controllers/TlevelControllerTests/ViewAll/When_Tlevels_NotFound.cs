using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class When_Tlevels_NotFound : TestSetup
    {
        public override void Given()
        {
            var mockresult = new YourTlevelsViewModel
            {
                IsAnyReviewPending = false,
                ConfirmedTlevels = new List<YourTlevelViewModel>(),
                QueriedTlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetYourTlevelsViewModel(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetYourTlevelsViewModel(Arg.Any<long>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
