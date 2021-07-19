using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.RegistrationCannotBeWithdrawn
{
    public class When_Cache_Found : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RegistrationCannotBeWithdrawnViewModel { ProfileId = 1 };
            CacheService.GetAndRemoveAsync<RegistrationCannotBeWithdrawnViewModel>(CacheKey).Returns(ViewModel);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            CacheService.Received(1).GetAndRemoveAsync<RegistrationCannotBeWithdrawnViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationCannotBeDeletedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(ViewModel.ProfileId);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AmendActiveRegistration);
            backLink.RouteAttributes.Count.Should().Be(2);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(ViewModel.ProfileId.ToString());
            backLink.RouteAttributes.TryGetValue(Constants.ChangeStatusId, out string changeStatusId);
            changeStatusId.Should().Be(((int)RegistrationChangeStatus.Withdrawn).ToString());
        }
    }
}
