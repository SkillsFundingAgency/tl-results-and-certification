using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderPost
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        public override void Given()
        {
            SelectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "1234567890", IsChangeMode = true };

            var cacheResult = new RegistrationViewModel
            {
                Uln = new UlnViewModel { Uln = "1234567890" },
                LearnersName = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" },
                DateofBirth = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" }
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationCore()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
            route.RouteValues[Constants.IsChangeMode].Should().Be("true");
        }
    }
}
