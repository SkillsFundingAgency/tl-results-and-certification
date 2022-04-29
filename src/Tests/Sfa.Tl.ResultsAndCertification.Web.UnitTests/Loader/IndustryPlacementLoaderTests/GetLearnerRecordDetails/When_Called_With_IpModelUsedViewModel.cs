using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetLearnerRecordDetails
{
    public class When_Called_With_IpModelUsedViewModel : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;
        protected IpModelUsedViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProviderUkprn = 9874561231;
            ProfileId = 1;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                LearnerName = "John Smith",
                IndustryPlacementStatus = IndustryPlacementStatus.Completed
            };
        }

        public async override Task When()
        {
            ActualResult = await Loader.TransformFromLearnerDetailsTo<IpModelUsedViewModel>(_ipCompletionViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(0).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_ipCompletionViewModel.ProfileId);
            ActualResult.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            ActualResult.IsIpModelUsed.Should().BeNull();
            ActualResult.IsValid.Should().BeTrue();
        }
    }
}
