using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeCoreGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeCoreViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new ChangeCoreViewModel
            {
                ProfileId = 1
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeCoreViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetProfile()
        {
            RegistrationLoader.Received(1).GetRegistrationProfileAsync<ChangeCoreViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeCoreViewModel));

            var model = viewResult.Model as ChangeCoreViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
