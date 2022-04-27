using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        protected AddMathsStatusViewModel ActualResult { get; set; }

        public override void Given()
        {
            _expectedApiResult = null;
            InternalApiClient.GetLearnerRecordDetailsAsync(Arg.Any<long>(), Arg.Any<int>()).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId);
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
