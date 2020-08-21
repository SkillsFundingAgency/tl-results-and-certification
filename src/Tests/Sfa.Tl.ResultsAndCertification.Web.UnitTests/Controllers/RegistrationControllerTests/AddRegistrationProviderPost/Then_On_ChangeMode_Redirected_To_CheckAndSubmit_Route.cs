using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderPost
{
    public class Then_On_ChangeMode_Redirected_To_CheckAndSubmit_Route : When_AddRegistrationProviderAsync_Post_Action_Is_Called
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
        public void Then_Redirected_To_AddRegistrationCheckAndSubmit_Route()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
            route.RouteValues[Constants.IsChangeMode].Should().Be("true");
        }
    }
}
