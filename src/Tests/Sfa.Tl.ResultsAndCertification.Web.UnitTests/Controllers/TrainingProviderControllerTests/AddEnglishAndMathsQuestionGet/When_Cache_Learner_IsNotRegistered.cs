using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsQuestionGet
{
    public class When_Cache_Learner_IsNotRegistered : TestSetup
    {
        private AddLearnerRecordViewModel cacheResult;

        public override void Given()
        {
            cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = new FindLearnerRecord { IsLearnerRegistered = false },
                Uln = new EnterUlnViewModel(),
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheResult);
        }
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
