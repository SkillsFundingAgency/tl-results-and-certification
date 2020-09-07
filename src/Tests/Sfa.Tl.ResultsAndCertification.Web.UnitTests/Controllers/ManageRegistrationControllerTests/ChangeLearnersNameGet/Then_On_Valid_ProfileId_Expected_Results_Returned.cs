using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNameGet
{
    public class Then_On_Valid_ProfileId_Expected_Results_Returned : When_ChangeLearnersNameAsync_Is_Called
    {
        private ChangeLearnersNameViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new ChangeLearnersNameViewModel
            {
                ProfileId = 1,
                Firstname = "John",
                Lastname = "Smith",
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeLearnersNameViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_RegistrationDetailsViewModel_Is_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeLearnersNameViewModel));

            var model = viewResult.Model as ChangeLearnersNameViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.Firstname.Should().Be(mockresult.Firstname);
            model.Lastname.Should().Be(mockresult.Lastname);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue("profileId", out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
