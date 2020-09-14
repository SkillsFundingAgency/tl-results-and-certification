using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.CancelRegistrationGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RegistrationDetailsViewModel mockresult;
        
        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel { Uln = 1234567890, ProfileId = 99 };
            RegistrationLoader.GetRegistrationDetailsByProfileIdAsync(Ukprn, ProfileId)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationDetailsByProfileIdAsync(Ukprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(CancelRegistrationViewModel));

            var model = viewResult.Model as CancelRegistrationViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.CancelRegistration.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            
            model.BackLink.RouteAttributes.TryGetValue("profileId", out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
