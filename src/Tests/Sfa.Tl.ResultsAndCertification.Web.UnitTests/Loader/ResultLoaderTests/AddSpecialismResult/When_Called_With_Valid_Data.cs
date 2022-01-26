using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddSpecialismResult
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AddResultResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            var lookupApiClientResponse = new List<LookupData> { new LookupData { Id = 1, Code = "SCG1", Value = "Merit" } };

            ViewModel = new ManageSpecialismResultViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                SelectedGradeCode = "SCG1",
                LookupId = 1
            };

            ExpectedApiResult = new AddResultResponse { IsSuccess = true, Uln = 1234567890, ProfileId = ProfileId };

            InternalApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(lookupApiClientResponse);

            InternalApiClient.AddResultAsync(Arg.Is<AddResultRequest>(
                                            x => x.ProfileId == ViewModel.ProfileId &&
                                            x.AoUkprn == AoUkprn &&
                                            x.ComponentType == ComponentType.Specialism &&
                                            x.LookupId == ViewModel.LookupId))
                                        .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetLookupData()
        {
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(ExpectedApiResult.IsSuccess);
            ActualResult.Uln.Should().Be(ExpectedApiResult.Uln);
            ActualResult.ProfileId.Should().Be(ExpectedApiResult.ProfileId);
        }
    }
}
