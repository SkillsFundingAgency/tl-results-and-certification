using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationGet
{
    public class Then_On_Invalid_ProfileId_Redirected_To_PageNotFound : When_CancelRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            RegistrationDetailsViewModel mockresult = null;
            RegistrationLoader.GetRegistrationDetailsByProfileIdAsync(Ukprn, ProfileId)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetRegistrationDetailsByProfileIdAsync_Is_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationDetailsByProfileIdAsync(Ukprn, ProfileId);
        }

        [Fact]
        public void Then_On_No_Record_Found_Redirect_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
