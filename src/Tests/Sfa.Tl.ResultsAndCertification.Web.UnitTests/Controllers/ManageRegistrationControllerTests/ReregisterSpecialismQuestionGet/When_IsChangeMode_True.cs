using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismQuestionGet
{
    public class When_IsChangeMode_True : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private RegistrationDetailsViewModel mockresult = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            // input variance.
            var isChangeStartedFromCore = false;
            IsChangeMode = true;

            // mock setup
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };
            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = new ReregisterCoreViewModel { IsChangeMode = isChangeStartedFromCore, CoreCodeAtTheTimeOfWithdrawn = "999", SelectedCoreCode = "123" },
                ReregisterProvider = new ReregisterProviderViewModel(),
                ReregisterAcademicYear = new ReregisterAcademicYearViewModel()
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterSpecialismQuestionViewModel));

            var model = viewResult.Model as ReregisterSpecialismQuestionViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.IsChangeMode.Should().BeTrue();
            model.IsChangeModeFromCore.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            backLink.RouteAttributes.Count.Should().Be(1);
            
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeParam1);
            routeParam1.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
