using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementPost
{
    public class When_Failed : TestSetup
    {
        private UpdateLearnerRecordResponseViewModel _updateLearnerRecordResponse;

        public override void Given()
        {
            _updateLearnerRecordResponse = new UpdateLearnerRecordResponseViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test User",
                IsModified = true,
                IsSuccess = false
            };

            ViewModel = new UpdateEnglishAndMathsQuestionViewModel
            {
                ProfileId = 1,                
                EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved
            };

            TrainingProviderLoader.ProcessEnglishAndMathsQuestionUpdateAsync(ProviderUkprn, ViewModel).Returns(_updateLearnerRecordResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).ProcessEnglishAndMathsQuestionUpdateAsync(ProviderUkprn, ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
