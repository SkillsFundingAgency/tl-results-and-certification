using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetManageCoreResult
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiLookupData = new List<LookupData>
            {
                new LookupData { Id = 1, Code = "C1", Value = "V1" },
                new LookupData { Id = 2, Code = "C2", Value = "V2" }
            };

            InternalApiClient.GetLookupDataAsync(LookupCategory.PathwayComponentGrade).Returns(expectedApiLookupData);

            expectedApiResultDetails = new ResultDetails
            {
                ProfileId = ProfileId,
                PathwayAssessmentId = AssessmentId,
                PathwayAssessmentSeries = "Summer 2021",
                PathwayLarId = "12345678",
                PathwayName = "Construction",
                PathwayResultId = null,
            };
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(expectedApiResultDetails.ProfileId);
            ActualResult.AssessmentId.Should().Be(expectedApiResultDetails.PathwayAssessmentId);
            ActualResult.AssessmentSeries.Should().Be(expectedApiResultDetails.PathwayAssessmentSeries);
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResultDetails.PathwayName} ({expectedApiResultDetails.PathwayLarId})");
            ActualResult.ResultId.Should().BeNull();
            ActualResult.SelectedGradeCode.Should().Be(string.Empty);
            ActualResult.PathwayPrsStatus.Should().Be(expectedApiResultDetails.PathwayPrsStatus);
            ActualResult.IsValid.Should().BeTrue();

            ActualResult.Grades.Should().NotBeNull();
            ActualResult.Grades.Count.Should().Be(expectedApiLookupData.Count);

            for (int i = 0; i < ActualResult.Grades.Count; i++)
            {
                ActualResult.Grades[i].Id.Should().Be(expectedApiLookupData[i].Id);
                ActualResult.Grades[i].Code.Should().Be(expectedApiLookupData[i].Code);
                ActualResult.Grades[i].Value.Should().Be(expectedApiLookupData[i].Value);
            }
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received(1).GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }
    }
}
