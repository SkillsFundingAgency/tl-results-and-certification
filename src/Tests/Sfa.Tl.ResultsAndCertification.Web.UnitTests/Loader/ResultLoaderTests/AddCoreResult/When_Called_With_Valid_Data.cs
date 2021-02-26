using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddCoreResult
{
    public class When_Called_With_Valid_Data : TestSetup
    {        
        private AddResultResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            var lookupApiClientResponse = new List<LookupData> { new LookupData { Id = 1, Code = "PCG1", Value = "A*" } };

            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                SelectedGradeCode = "PCG1",
                LookupId = 1
            };

            
            ExpectedApiResult = new AddResultResponse { IsSuccess = true, Uln = 1234567890, ProfileId = ProfileId };
            
            InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(lookupApiClientResponse);

            InternalApiClient
                .AddResultAsync(Arg.Is<AddResultRequest>(
                    x => x.ProfileId == ViewModel.ProfileId &&
                    x.AoUkprn == AoUkprn &&
                    x.ComponentType == ComponentType.Core &&
                    x.LookupId == ViewModel.LookupId))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Recieved_Call_To_GetLookupData()
        {
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
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
