using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsQuestionPost
{
    public class When_Success : TestSetup
    {
        private AddLearnerRecordViewModel _cacheResult;
        private EnterUlnViewModel ulnViewModel;
        private FindLearnerRecord learnerRecord;

        public override void Given()
        {
            learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = true, IsLearnerRecordAdded = false };
            ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            EnglishAndMathsQuestionViewModel = new EnglishAndMathsQuestionViewModel { EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved };

            _cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = learnerRecord,
                Uln = ulnViewModel
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, _cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddIndustryPlacementQuestion()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddIndustryPlacementQuestion);
        }
    }
}
