using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.LearnerRecordDetailsGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private LearnerRecordDetailsViewModel mockresult = null;
        public override void Given()
        {
            ProfileId = 10;
            mockresult = new LearnerRecordDetailsViewModel
            {
                ProfileId = 10,
                Uln = 1235469874,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College (58794528)",
                PathwayName = "Test Pathway Name (97453214)",
                IsLearnerRegistered = true,
                IsLearnerRecordAdded = true,
                IsEnglishAndMathsAchieved = true,
                IsSendLearner = false,
                HasLrsEnglishAndMaths = true,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
            TrainingProviderLoader.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as LearnerRecordDetailsViewModel;
                        
            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.DateofBirth.Should().Be(mockresult.DateofBirth);
            model.ProviderName.Should().Be(mockresult.ProviderName);
            model.PathwayName.Should().Be(mockresult.PathwayName);
            model.IsLearnerRegistered.Should().Be(mockresult.IsLearnerRegistered);
            model.IsLearnerRecordAdded.Should().Be(mockresult.IsLearnerRecordAdded);
            model.IsEnglishAndMathsAchieved.Should().Be(mockresult.IsEnglishAndMathsAchieved);            
            model.HasLrsEnglishAndMaths.Should().Be(mockresult.HasLrsEnglishAndMaths);
            model.IsSendLearner.Should().Be(mockresult.IsSendLearner);
            model.IndustryPlacementId.Should().Be(mockresult.IndustryPlacementId);
            model.IndustryPlacementStatus.Should().Be(mockresult.IndustryPlacementStatus);
        }
    }
}
