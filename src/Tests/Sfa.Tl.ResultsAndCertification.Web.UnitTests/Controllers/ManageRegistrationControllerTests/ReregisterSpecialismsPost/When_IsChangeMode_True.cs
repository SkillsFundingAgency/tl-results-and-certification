using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsPost
{
    public class When_IsChangeMode_True : TestSetup
    {
        private ReregisterViewModel cacheResult;

        public override void Given()
        {
            ViewModel.IsChangeMode = true; 

            cacheResult = new ReregisterViewModel { SpecialismQuestion = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false } };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_HasLearnerDecidedSpecialism_Set_True()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x => x.SpecialismQuestion.HasLearnerDecidedSpecialism == true));
        }
        
        [Fact]
        public void Then_Redirected_To_ReregisterCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            route.RouteValues.Count.Should().Be(1);
            routeName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
