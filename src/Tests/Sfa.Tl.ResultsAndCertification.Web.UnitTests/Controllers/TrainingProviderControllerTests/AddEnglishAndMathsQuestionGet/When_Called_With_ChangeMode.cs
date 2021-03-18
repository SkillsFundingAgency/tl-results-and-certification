using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsQuestionGet
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        private AddLearnerRecordViewModel _cacheResult;
        private EnterUlnViewModel _ulnViewModel;
        private FindLearnerRecord _learnerRecord;
        private EnglishAndMathsQuestionViewModel _englishAndMathsQuestionViewModel;
        private IndustryPlacementQuestionViewModel _industryPlacementQuestionViewModel;

        public override void Given()
        {
            IsChangeMode = true;
            _learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = true, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = false };
            _ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            _englishAndMathsQuestionViewModel = new EnglishAndMathsQuestionViewModel { LearnerName = _learnerRecord.Name, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved };
            _industryPlacementQuestionViewModel = new IndustryPlacementQuestionViewModel { LearnerName = _learnerRecord.Name, IndustryPlacementStatus = IndustryPlacementStatus.Completed };

            _cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = _learnerRecord,
                Uln = _ulnViewModel,
                EnglishAndMathsQuestion = _englishAndMathsQuestionViewModel,
                IndustryPlacementQuestion = _industryPlacementQuestionViewModel
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(_cacheResult);
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
            model.EnglishAndMathsStatus.Should().Be(_englishAndMathsQuestionViewModel.EnglishAndMathsStatus);
            model.LearnerName.Should().Be(_learnerRecord.Name);
            model.IsChangeMode.Should().Be(IsChangeMode);
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddLearnerRecordCheckAndSubmit);
        }
    }
}
