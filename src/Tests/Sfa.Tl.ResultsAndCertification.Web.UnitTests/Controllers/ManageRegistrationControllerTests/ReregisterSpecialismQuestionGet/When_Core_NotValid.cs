using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismQuestionGet
{
    public class When_Core_NotValid : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private RegistrationDetailsViewModel mockresult = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus,
            };

            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = new ReregisterCoreViewModel
                {
                    CoreCodeAtTheTimeOfWithdrawn = "999",
                    SelectedCoreCode = "999"
                }
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
