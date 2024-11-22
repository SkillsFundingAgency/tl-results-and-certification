using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminProviderLoaderTests
{
    public class When_GetProviderDetailsViewModel_Called_Returns_Expected : AdminProviderLoaderBaseTest
    {
        private const int ProviderId = 1;
        private AdminProviderDetailsViewModel _result;

        private readonly GetProviderResponse _apiResponse = new()
        {
            ProviderId = ProviderId,
            UkPrn = 10000712,
            Name = "University College Birmingham",
            DisplayName = "University College Birmingham",
            IsActive = true
        };

        public override void Given()
        {
            ApiClient.GetProviderAsync(ProviderId).Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetProviderDetailsViewModel(ProviderId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetProviderAsync(ProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.ProviderId.Should().Be(ProviderId);
            _result.ProviderName.Should().Be(_apiResponse.Name);

            _result.SummaryUkprn.Id.Should().Be("ukprn");
            _result.SummaryUkprn.Title.Should().Be(AdminProviderDetails.Label_Ukprn);
            _result.SummaryUkprn.Value.Should().Be(_apiResponse.UkPrn.ToString());

            _result.SummaryName.Id.Should().Be("name");
            _result.SummaryName.Title.Should().Be(AdminProviderDetails.Label_Name);
            _result.SummaryName.Value.Should().Be(_apiResponse.Name);

            _result.SummaryDisplayName.Id.Should().Be("displayname");
            _result.SummaryDisplayName.Title.Should().Be(AdminProviderDetails.Label_Display_Name);
            _result.SummaryDisplayName.Value.Should().Be(_apiResponse.DisplayName);

            _result.SummaryIsActive.Id.Should().Be("active");
            _result.SummaryIsActive.Title.Should().Be(AdminProviderDetails.Label_Active);
            _result.SummaryIsActive.Value.Should().Be(AdminProviderDetails.Label_Yes);

            _result.BackLink.RouteName.Should().Be(RouteConstants.AdminFindProvider);
            _result.BackLink.RouteAttributes.Should().BeEmpty();

            _result.SuccessBanner.Should().BeNull();
        }
    }
}
