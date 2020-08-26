using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCheckAndSubmitGet
{
    public class Then_Validate_CheckAndSubmit_Page : When_AddRegistrationCheckAndSubmit_Action_Is_Called
    {
        public override void Given() { }

        public override void When() { }

        [Theory]
        [ClassData(typeof(CheckAndSubmitTestDataGenerator))]
        public void Then_Expected_Results_Returned(CheckAndSubmitTestDataModel data)
        {
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(data.RegistrationViewModel);

            Result = Controller.AddRegistrationCheckAndSubmitAsync().Result;

            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(data.RouteName);
        }
    }
}
