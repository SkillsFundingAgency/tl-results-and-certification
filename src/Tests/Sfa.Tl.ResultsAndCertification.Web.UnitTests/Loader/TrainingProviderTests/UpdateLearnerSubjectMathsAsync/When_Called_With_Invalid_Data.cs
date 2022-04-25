using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.UpdateLearnerSubjectMathsAsync
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private bool _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            AddMathsStatusViewModel = new AddMathsStatusViewModel
            {
                ProfileId = 1,
                LearnerName = "John Smith",
                IsAchieved = true,
            };

            _expectedApiResult = false;

            InternalApiClient.UpdateLearnerSubjectAsync(Arg.Any<UpdateLearnerSubjectRequest>())
                .Returns(_expectedApiResult);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
