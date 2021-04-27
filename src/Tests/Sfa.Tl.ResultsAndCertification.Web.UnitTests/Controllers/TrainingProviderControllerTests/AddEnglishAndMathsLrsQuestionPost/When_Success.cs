using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishAndMathsLrsQuestionPost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            LearnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Barnsley College (123456789)", IsLearnerRegistered = true, IsLearnerRecordAdded = false, HasLrsEnglishAndMaths = false };
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };
            EnglishAndMathsLrsQuestionViewModel = new EnglishAndMathsLrsQuestionViewModel { EnglishAndMathsLrsStatus = EnglishAndMathsLrsStatus.AchievedWithSend };

            AddLearnerRecordViewModel = new AddLearnerRecordViewModel
            {
                LearnerRecord = LearnerRecord,
                Uln = EnterUlnViewModel,
                EnglishAndMathsLrsQuestion = EnglishAndMathsLrsQuestionViewModel
            };

            AddLearnerRecordResponse = new AddLearnerRecordResponse
            {
                IsSuccess = true,
                Uln = LearnerRecord.Uln,
                Name = LearnerRecord.Name
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(AddLearnerRecordViewModel);
            TrainingProviderLoader.AddLearnerRecordAsync(ProviderUkprn, AddLearnerRecordViewModel).Returns(AddLearnerRecordResponse);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).AddLearnerRecordAsync(ProviderUkprn, AddLearnerRecordViewModel);
            CacheService.DidNotReceive().RemoveAsync<SearchLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).RemoveAsync<AddLearnerRecordViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.AddEnglishAndMathsSendDataConfirmation),
                Arg.Is<LearnerRecordConfirmationViewModel>
                (x => x.Name == AddLearnerRecordViewModel.LearnerRecord.Name &&
                      x.Uln == AddLearnerRecordViewModel.LearnerRecord.Uln),
                 CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AddEnglishAndMathsSendDataConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddEnglishAndMathsSendDataConfirmation);
        }
    }
}
