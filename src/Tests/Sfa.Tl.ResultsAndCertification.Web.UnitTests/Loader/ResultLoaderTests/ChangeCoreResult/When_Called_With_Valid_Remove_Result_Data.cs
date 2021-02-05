using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ChangeCoreResult
{
    public class When_Called_With_Valid_Remove_Result_Data : TestSetup
    {
        private ChangeResultResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            //var lookupApiClientResponse = new List<LookupData> { new LookupData { Id = 1, Code = "PCG1", Value = "A*" } };

            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = ProfileId,
                ResultId = 1,
                SelectedGradeCode = string.Empty
            };


            ExpectedApiResult = new ChangeResultResponse { IsSuccess = true, Uln = 1234567890, ProfileId = ProfileId };

            //InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(lookupApiClientResponse);

            InternalApiClient
                .ChangeResultAsync(Arg.Is<ChangeResultRequest>(
                    x => x.ProfileId == ViewModel.ProfileId &&
                    x.AoUkprn == AoUkprn &&
                    x.ResultId == ViewModel.ResultId &&
                    x.ComponentType == ComponentType.Core &&
                    x.LookupId == ViewModel.LookupId))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Not_Recieved_Call_To_GetLookupData()
        {
            InternalApiClient.DidNotReceive().GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
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
