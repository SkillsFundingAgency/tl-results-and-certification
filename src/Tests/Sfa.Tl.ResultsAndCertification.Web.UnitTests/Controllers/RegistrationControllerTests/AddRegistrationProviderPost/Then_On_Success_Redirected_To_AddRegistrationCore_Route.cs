using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderPost
{
    public class Then_On_Success_Redirected_To_AddRegistrationCore_Route : When_AddRegistrationProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            SelectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "1234567890" };

            var cacheResult = new RegistrationViewModel
            {
                Uln = new UlnViewModel { Uln = "1234567890" },
                LearnersName = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" },
                DateofBirth = new DateofBirthViewModel { Day = DateTime.Now.Day.ToString(), Month = DateTime.Now.Month.ToString(), Year = DateTime.Now.Year.ToString() }
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_On_Success_Redirected_To_AddRegistration_Core_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationCore);
        }
    }
}
