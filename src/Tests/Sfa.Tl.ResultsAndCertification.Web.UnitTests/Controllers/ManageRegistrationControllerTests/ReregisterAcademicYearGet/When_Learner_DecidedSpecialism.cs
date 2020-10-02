using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearGet
{
    public class When_Learner_DecidedSpecialism : TestSetup
    {
        public override void Given()
        {
            var specialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            var cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = specialismQuestionViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
