using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class Then_No_Results_Redirect_To_PageNotFound : When_ViewAll_Action_Called
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
        public void Then_GetAllTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetYourTlevelsViewModel(Arg.Any<long>());
        }

        [Fact]
        public void Then_NoResults_ViewModel_Redirected_To_Route_PageNotFound()
        {
            var routeName = (Result.Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
