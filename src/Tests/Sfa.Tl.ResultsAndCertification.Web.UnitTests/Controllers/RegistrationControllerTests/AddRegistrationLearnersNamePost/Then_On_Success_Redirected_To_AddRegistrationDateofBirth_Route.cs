using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNamePost
{
    public class Then_On_Success_Redirected_To_AddRegistrationDateofBirth_Route : When_AddRegistrationLearnersName_Post_Action_Is_Called
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
        public void Then_On_Success_Redirected_To_DateofBirth_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationDateofBirth);
        }
    }
}
