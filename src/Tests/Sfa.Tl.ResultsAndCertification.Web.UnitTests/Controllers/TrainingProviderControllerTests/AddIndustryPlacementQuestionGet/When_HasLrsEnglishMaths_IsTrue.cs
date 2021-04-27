using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddIndustryPlacementQuestionGet
{
    public class When_HasLrsEnglishMaths_IsTrue : TestSetup
    {
        private AddLearnerRecordViewModel _cacheResult;
        private FindLearnerRecord _learnerRecord;

        public override void Given()
        {
            _learnerRecord = new FindLearnerRecord { IsLearnerRegistered = true, HasLrsEnglishAndMaths = true };
            _cacheResult = new AddLearnerRecordViewModel { LearnerRecord = _learnerRecord, Uln = new EnterUlnViewModel() };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_BackLink_Set_To_EnterUniqueLearnerNumber()
        {
            Result.Should().NotBeNull();
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IndustryPlacementQuestionViewModel;
            model.Should().NotBeNull();
            
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumber);
        }
    }
}
