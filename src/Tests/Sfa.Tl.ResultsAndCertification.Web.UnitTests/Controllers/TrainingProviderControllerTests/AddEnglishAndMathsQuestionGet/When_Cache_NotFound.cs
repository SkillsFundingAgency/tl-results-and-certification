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
    public class When_Cache_NotFound : TestSetup
    {
        private AddLearnerRecordViewModel cacheResult;
        private EnterUlnViewModel ulnViewModel;
        private FindLearnerRecord learnerRecord;

        public override void Given()
        {
            learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = true };
            ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = learnerRecord,
                Uln = ulnViewModel,
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
            viewResult.Model.Should().BeOfType(typeof(EnglishAndMathsQuestionViewModel));

            var model = viewResult.Model as EnglishAndMathsQuestionViewModel;
            model.Should().NotBeNull();
            model.EnglishAndMathsStatus.Should().BeNull();
            model.LearnerName.Should().Be(learnerRecord.Name);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumber);
        }
    }
}
