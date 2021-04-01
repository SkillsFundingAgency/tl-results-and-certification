using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;        

        public override void Given()
        {
            _expectedApiResult = null;
            InternalApiClient.GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
