using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDownloadLearnerResultsLoaderTests
{
    public class When_GetDownloadLearnerResultsByProviderViewModel_Called_Returns_Expected : AdminDownloadLearnerResultsLoaderBaseTest
    {
        private readonly int ProviderId = 99;

        private AdminDownloadLearnerResultsByProviderViewModel _result;

        private readonly AdminDownloadLearnerResultsByProviderViewModel _expectedResult = new()
        {
            ProviderUkprn = 10004576,
            ProviderName = "New College Durham"
        };

        public override void Given()
        {
            var getProviderResponse = new GetProviderResponse
            {
                UkPrn = _expectedResult.ProviderUkprn,
                DisplayName = _expectedResult.ProviderName
            };

            ApiClient.GetProviderAsync(ProviderId).Returns(getProviderResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetDownloadLearnerResultsByProviderViewModel(ProviderId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetProviderAsync(ProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_expectedResult);
        }
    }
}