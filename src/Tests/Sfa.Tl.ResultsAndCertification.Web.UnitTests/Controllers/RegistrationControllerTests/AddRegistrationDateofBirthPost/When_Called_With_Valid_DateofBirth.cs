using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthPost
{
    public class When_Called_With_Valid_DateofBirth : TestSetup
    {
        private RegistrationViewModel cacheModel;

        public override void Given()
        {
            cacheModel = new RegistrationViewModel { LearnersName = new LearnersNameViewModel() };
            DateofBirthViewmodel = new DateofBirthViewModel { Day = "01", Month="01", Year = "2020" };
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<RegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<RegistrationViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationProvider()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationProvider);
        }
    }
}
