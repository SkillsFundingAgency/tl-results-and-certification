using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_IsStatusCompleted_True : TestSetup
    {
        public override void Given()
        {
            ProfileId = 10;
            Mockresult = new LearnerRecordDetailsViewModel
            {
                MathsStatus = SubjectStatus.Achieved,
                EnglishStatus = SubjectStatus.Achieved,
                IsLearnerRegistered = true,
                
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(ProviderUkprn, ProfileId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_IsStatusCompleted_IsTrue()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as LearnerRecordDetailsViewModel;

            model.IsMathsAdded.Should().BeTrue();
            model.IsEnglishAdded.Should().BeTrue();
            model.IsStatusCompleted.Should().BeTrue();
        }
    }
}
