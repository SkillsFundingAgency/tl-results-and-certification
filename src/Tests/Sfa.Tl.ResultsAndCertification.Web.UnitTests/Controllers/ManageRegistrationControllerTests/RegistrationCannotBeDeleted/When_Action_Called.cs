using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.RegistrationCannotBeDeleted
{
    public class When_Action_Called : TestSetup
    {
        public override void Given()
        {
            RegistrationCannotBeDeletedViewModel = new RegistrationCannotBeDeletedViewModel { ProfileId = 1 };
            CacheService.GetAndRemoveAsync<RegistrationCannotBeDeletedViewModel>(string.Concat(CacheKey, Constants.RegistrationCannotBeDeletedViewModel))
                .Returns(RegistrationCannotBeDeletedViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RegistrationCannotBeDeletedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(RegistrationCannotBeDeletedViewModel.ProfileId);

            // Backlink
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AmendActiveRegistration);
            backLink.RouteAttributes.Count.Should().Be(2);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileId);
            profileId.Should().Be(RegistrationCannotBeDeletedViewModel.ProfileId.ToString());
            backLink.RouteAttributes.TryGetValue(Constants.ChangeStatusId, out string changeStatusId);
            changeStatusId.Should().Be(((int)RegistrationChangeStatus.Delete).ToString());
        }
    }
}
