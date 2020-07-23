using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthPost
{
    public class Then_On_Valid_DateofBirth_Redirected_To_Route_AddRegistrationProvider : When_AddRegistrationDateofBirth_Post_Action_Is_Called
    {
        private RegistrationViewModel cacheModel;

        public override void Given()
        {
            cacheModel = new RegistrationViewModel { LearnersName = new LearnersNameViewModel() };
            DateofBirthViewmodel = new DateofBirthViewModel { Day = "01", Month="01", Year = "2020" };
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheModel);
        }

        [Fact]
        public void Then_DateofBirth_Cache_Is_Synchronised()
        {
            CacheService.Received(1).GetAsync<RegistrationViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<RegistrationViewModel>());
        }

        [Fact]
        public void Then_Valid_DateofBirth_Redirected_ToAddRegistrationLearnerName_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationProvider);
        }
    }
}
