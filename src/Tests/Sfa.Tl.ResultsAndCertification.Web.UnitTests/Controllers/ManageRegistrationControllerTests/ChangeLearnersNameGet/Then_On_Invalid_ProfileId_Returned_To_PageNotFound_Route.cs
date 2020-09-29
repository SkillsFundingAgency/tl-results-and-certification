using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNameGet
{
    public class Then_On_Invalid_ProfileId_Returned_To_PageNotFound_Route : When_ChangeLearnersNameAsync_Is_Called
    {
        private ChangeLearnersNameViewModel mockresult = null;
        public override void Given()
        {
            RegistrationLoader.GetRegistrationProfileAsync<ChangeLearnersNameViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_To_Return_PageNotFound_Page()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
