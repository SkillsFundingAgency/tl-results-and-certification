﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.AddLearnerRecord
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddLearnerRecordResponse _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;           

            AddLearnerRecordViewModel = new AddLearnerRecordViewModel
            {
                LearnerRecord = new Models.Contracts.TrainingProvider.FindLearnerRecord { Uln = 1234567890, Name = "Test Name", HasLrsEnglishAndMaths = false },
                Uln = new EnterUlnViewModel { EnterUln = "1234567890" },
                EnglishAndMathsQuestion = new EnglishAndMathsQuestionViewModel { EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend },
                IndustryPlacementQuestion = new IndustryPlacementQuestionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.Completed }
            };

            _expectedApiResult = new AddLearnerRecordResponse { Uln = AddLearnerRecordViewModel.LearnerRecord.Uln, Name = AddLearnerRecordViewModel.LearnerRecord.Name, IsSuccess = true };

            InternalApiClient.AddLearnerRecordAsync(Arg.Is<AddLearnerRecordRequest>(
                    x => x.Uln == AddLearnerRecordViewModel.LearnerRecord.Uln &&
                    x.Ukprn == ProviderUkprn &&
                    x.EnglishAndMathsStatus == AddLearnerRecordViewModel.EnglishAndMathsQuestion.EnglishAndMathsStatus &&
                    x.HasLrsEnglishAndMaths == AddLearnerRecordViewModel.LearnerRecord.HasLrsEnglishAndMaths &&
                    x.IndustryPlacementStatus == AddLearnerRecordViewModel.IndustryPlacementQuestion.IndustryPlacementStatus &&
                    x.PerformedBy == $"{Givenname} {Surname}" &&
                    x.PerformedUserEmail == Email))
                .Returns(_expectedApiResult);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Name.Should().Be(_expectedApiResult.Name);
            ActualResult.IsSuccess.Should().Be(_expectedApiResult.IsSuccess);
        }
    }
}
