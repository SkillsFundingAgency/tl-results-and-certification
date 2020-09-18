using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeDateofBirthGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeDateofBirthViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new ChangeDateofBirthViewModel
            {
                ProfileId = 1,
                Day = "1",
                Month = "2",
                Year = "2000"
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeDateofBirthViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeDateofBirthViewModel));

            var model = viewResult.Model as ChangeDateofBirthViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.Day.Should().Be(mockresult.Day);
            model.Month.Should().Be(mockresult.Month);
            model.Year.Should().Be(mockresult.Year);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
