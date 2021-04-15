using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private UpdateIndustryPlacementQuestionViewModel mockresult = null;

        public override void Given()
        {
            ProfileId = 10;
            PathwayId = 15;
            mockresult = new UpdateIndustryPlacementQuestionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = PathwayId,
                LearnerName = "Test user",
                IsLearnerRecordAdded = true,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<UpdateIndustryPlacementQuestionViewModel>(ProviderUkprn, ProfileId, PathwayId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<UpdateIndustryPlacementQuestionViewModel>(ProviderUkprn, ProfileId, PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as UpdateIndustryPlacementQuestionViewModel;

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.RegistrationPathwayId.Should().Be(mockresult.RegistrationPathwayId);
            model.LearnerName.Should().Be(mockresult.LearnerName);
            model.IsLearnerRecordAdded.Should().Be(mockresult.IsLearnerRecordAdded);
            model.IndustryPlacementStatus.Should().Be(mockresult.IndustryPlacementStatus);
        }
    }
}
