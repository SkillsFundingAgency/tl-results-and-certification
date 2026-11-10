using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitGet
{
    public class When_Validate_Reregister_CheckAndSubmit : TestSetup
    {
        public override void Given() { }

        [Theory]
        [ClassData(typeof(ReregisterCheckAndSubmitTestDataGenerator))]
        public async Task Then_Returns_Expected_Results(ReregisterCheckAndSubmitTestDataModel data)
        {
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(data.ReregisterViewModel);

            Result = await Controller.ReregisterCheckAndSubmitAsync(data.ProfileId);

            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(data.RouteName);
        }
    }
}
