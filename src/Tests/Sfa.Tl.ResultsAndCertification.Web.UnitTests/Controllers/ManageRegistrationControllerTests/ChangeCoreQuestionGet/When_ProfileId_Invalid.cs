using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeCoreQuestionGet
{
    public class When_ProfileId_Invalid : TestSetup
    {
        private ChangeProviderViewModel cacheResult;
        private ChangeCoreQuestionViewModel mockresult = null;

        public override void Given()
        {
            cacheResult = new ChangeProviderViewModel
            {
                SelectedProviderDisplayName = "Test (12345678)"
            };

            CacheService.GetAndRemoveAsync<ChangeProviderViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegistrationChangeCoreQuestionDetailsAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
