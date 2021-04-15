using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private UpdateEnglishAndMathsQuestionViewModel mockresult = null;

        public override void Given()
        {
            ProfileId = 10;            
            mockresult = new UpdateEnglishAndMathsQuestionViewModel
            {
                ProfileId = ProfileId,                
                LearnerName = "Test user",
                IsLearnerRecordAdded = true,
                EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateEnglishAndMathsQuestionViewModel>(ProviderUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<UpdateEnglishAndMathsQuestionViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as UpdateEnglishAndMathsQuestionViewModel;

            model.ProfileId.Should().Be(mockresult.ProfileId);            
            model.LearnerName.Should().Be(mockresult.LearnerName);
            model.IsLearnerRecordAdded.Should().Be(mockresult.IsLearnerRecordAdded);
            model.EnglishAndMathsStatus.Should().Be(mockresult.EnglishAndMathsStatus);
        }
    }
}
