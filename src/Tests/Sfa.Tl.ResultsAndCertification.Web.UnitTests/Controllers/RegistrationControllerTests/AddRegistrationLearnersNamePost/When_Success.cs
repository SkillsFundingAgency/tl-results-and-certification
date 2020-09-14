using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNamePost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            UlnViewModel = new UlnViewModel { Uln = "1234567890" };
            LearnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };

            var cacheResult = new RegistrationViewModel
            {
                Uln = UlnViewModel,
                LearnersName = LearnersNameViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationDateofBirth()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationDateofBirth);
        }
    }
}
