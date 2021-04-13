using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementGet
{
    public class When_Called_With_EnglishAndMathsStatus_Null : TestSetup
    {
        private UpdateEnglishAndMathsQuestionViewModel mockresult = null;

        public override void Given()
        {
            ProfileId = 10;
            mockresult = new UpdateEnglishAndMathsQuestionViewModel
            {
                ProfileId = ProfileId,
                IsLearnerRecordAdded = true,
                HasLrsEnglishAndMaths = false,
                EnglishAndMathsStatus = null
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateEnglishAndMathsQuestionViewModel>(ProviderUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<UpdateEnglishAndMathsQuestionViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
