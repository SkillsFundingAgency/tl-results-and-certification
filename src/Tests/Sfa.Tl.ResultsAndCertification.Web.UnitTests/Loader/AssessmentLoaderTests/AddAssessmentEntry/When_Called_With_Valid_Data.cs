﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.AddAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddAssessmentEntryResponse ExpectedResult { get; set; }
        private AddAssessmentEntryResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new AddAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                AssessmentSeriesId = 1,
                ComponentType = Common.Enum.ComponentType.Core,
            };

            ExpectedApiResult = new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1234567890 }; 
            InternalApiClient
                .AddAssessmentEntryAsync(Arg.Is<AddAssessmentEntryRequest>(
                    x => x.ProfileId == ViewModel.ProfileId && 
                    x.AoUkprn == AoUkprn && 
                    x.ComponentType == Common.Enum.ComponentType.Core &&
                    x.AssessmentSeriesId == ViewModel.AssessmentSeriesId))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(ExpectedApiResult.IsSuccess);
            ActualResult.Uln.Should().Be(ExpectedApiResult.Uln);
        }
    }
}
