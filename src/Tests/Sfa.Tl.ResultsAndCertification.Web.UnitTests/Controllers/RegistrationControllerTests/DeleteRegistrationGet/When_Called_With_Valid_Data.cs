using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.DeleteRegistrationGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentDetailsViewModel mockresult;

        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel { Uln = 1234567890, ProfileId = 99, IsResultExist = false };
            RegistrationLoader.GetRegistrationAssessmentAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationAssessmentAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(DeleteRegistrationViewModel));

            var model = viewResult.Model as DeleteRegistrationViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.DeleteRegistration.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AmendActiveRegistration);
            model.BackLink.RouteAttributes.Count.Should().Be(2);

            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValueProfileId);
            routeValueProfileId.Should().Be(mockresult.ProfileId.ToString());
            model.BackLink.RouteAttributes.TryGetValue(Constants.ChangeStatusId, out string routeValueChangeStatus);
            routeValueChangeStatus.Should().Be(((int)RegistrationChangeStatus.Delete).ToString());
        }
    }
}
