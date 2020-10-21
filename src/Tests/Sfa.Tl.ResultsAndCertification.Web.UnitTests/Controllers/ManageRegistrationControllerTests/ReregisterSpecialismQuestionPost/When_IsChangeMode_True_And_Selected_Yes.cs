using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismQuestionPost
{
    public class When_IsChangeMode_True_And_Selected_Yes : TestSetup
    {
        public override void Given()
        {
            ViewModel = new ReregisterSpecialismQuestionViewModel 
            {
                ProfileId = 11,
                HasLearnerDecidedSpecialism = true, 
                IsChangeMode = true 
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(new ReregisterViewModel { ReregisterCore = new ReregisterCoreViewModel() });
        }

        [Fact]
        public void Then_Redirected_To_ReregisterSpecialisms()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            route.RouteValues.Count.Should().Be(2);

            routeName.Should().Be(RouteConstants.ReregisterSpecialisms);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            
            routeName.Should().Be(RouteConstants.ReregisterSpecialisms);
            route.RouteValues[Constants.IsChangeMode].Should().Be("true");
        }
    }
}
