using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultDetails
{
    public class When_Called_With_Status_Withdrawn : TestSetup
    {
        public override void Given()
        {
            CoreStatus = RegistrationPathwayStatus.Withdrawn;
            expectedApiResult = new LearnerRecord
            {
                ProfileId = 1,
                Pathway = new Pathway
                {
                    Id = 2,
                    Status = CoreStatus,
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}