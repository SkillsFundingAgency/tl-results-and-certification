using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismQuestionGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeSpecialismQuestionViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new ChangeSpecialismQuestionViewModel
            {
                ProfileId = 1,
                HasLearnerDecidedSpecialism = true
            };

            RegistrationLoader.GetRegistrationProfileAsync<ChangeSpecialismQuestionViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetProfile()
        {
            RegistrationLoader.Received(1).GetRegistrationProfileAsync<ChangeSpecialismQuestionViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeSpecialismQuestionViewModel));

            var model = viewResult.Model as ChangeSpecialismQuestionViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.HasLearnerDecidedSpecialism.Should().BeTrue();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.RegistrationDetails);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
