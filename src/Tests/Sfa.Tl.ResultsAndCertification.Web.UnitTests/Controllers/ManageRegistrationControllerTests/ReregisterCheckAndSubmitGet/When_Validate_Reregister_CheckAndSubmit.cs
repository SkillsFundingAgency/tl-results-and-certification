using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitGet
{
    public class When_Validate_Reregister_CheckAndSubmit : TestSetup
    {
        public override void Given() { }

        [Theory]
        [ClassData(typeof(ReregisterCheckAndSubmitTestDataGenerator))]
        public void Then_Returns_Expected_Results(ReregisterCheckAndSubmitTestDataModel data)
        {
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(data.ReregisterViewModel);

            Result = Controller.ReregisterCheckAndSubmitAsync(data.ProfileId).Result;

            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(data.RouteName);
        }
    }
}
