using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsLrsQuestionGet
{
    public class When_Cache_IsFound : TestSetup
    {
        private AddLearnerRecordViewModel cacheResult;
        private EnterUlnViewModel ulnViewModel;
        private FindLearnerRecord learnerRecord;
        private EnglishAndMathsLrsQuestionViewModel englishAndMathsLrsQuestionViewModel;

        public override void Given()
        {
            learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsSendConfirmationRequired = true };
            ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            englishAndMathsLrsQuestionViewModel = new EnglishAndMathsLrsQuestionViewModel { LearnerName = "Test Name", EnglishAndMathsLrsStatus = EnglishAndMathsLrsStatus.AchievedWithSend };

            cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = learnerRecord,
                Uln = ulnViewModel,
                EnglishAndMathsLrsQuestion = englishAndMathsLrsQuestionViewModel,
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(EnglishAndMathsLrsQuestionViewModel));

            var model = viewResult.Model as EnglishAndMathsLrsQuestionViewModel;
            model.Should().NotBeNull();
            model.EnglishAndMathsLrsStatus.Should().Be(englishAndMathsLrsQuestionViewModel.EnglishAndMathsLrsStatus);
            model.LearnerName.Should().Be(learnerRecord.Name);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumber);
        }
    }
}
