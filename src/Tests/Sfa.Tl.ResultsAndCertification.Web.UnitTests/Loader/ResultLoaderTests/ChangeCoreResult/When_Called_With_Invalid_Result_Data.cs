using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ChangeCoreResult
{
    public class When_Called_With_Invalid_Result_Data : TestSetup
    {
        private ChangeResultResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            var lookupApiClientResponse = new List<LookupData> { new LookupData { Id = 1, Code = "PCG1", Value = "A*" } };

            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = ProfileId,
                ResultId = 1,
                SelectedGradeCode = "PCG10",
                LookupId = 1
            };

            ExpectedApiResult = new ChangeResultResponse { IsSuccess = true, Uln = 1234567890, ProfileId = ProfileId };

            InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(lookupApiClientResponse);

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
        public void Then_Recieved_Call_To_GetLookupData()
        {
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }

        [Fact]
        public void Then_NotRecieved_Call_To_ChangeResult()
        {
            InternalApiClient.DidNotReceive().ChangeResultAsync(new ChangeResultRequest());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();            
        }
    }
}
