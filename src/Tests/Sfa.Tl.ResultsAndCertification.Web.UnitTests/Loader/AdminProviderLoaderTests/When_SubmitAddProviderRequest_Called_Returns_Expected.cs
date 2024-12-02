using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminProviderLoaderTests
{
    public class When_SubmitAddProviderRequest_Called_Returns_Expected : AdminProviderLoaderBaseTest
    {
        private readonly AdminAddProviderViewModel _request = new()
        {
            UkPrn = "12345678",
            Name = "added-name",
            DisplayName = "added-display-name",
        };

        private readonly Expression<Predicate<AddProviderRequest>> _apiRequestPredicate =
            r => r.UkPrn == 12345678
            && r.Name == "added-name"
            && r.DisplayName == "added-display-name"
            && r.CreatedBy == "test user";

        private readonly AddProviderResponse _apiResponse = new()
        {
            ProviderId = 1,
            DuplicatedUkprnFound = false,
            DuplicatedNameFound = false,
            DuplicatedDisplayNameFound = false,
            Success = true
        };

        private AddProviderResponse _result;

        public override void Given()
        {
            ApiClient
                .AddProviderAsync(Arg.Is(_apiRequestPredicate))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SubmitAddProviderRequest(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).AddProviderAsync(Arg.Is(_apiRequestPredicate));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEquivalentTo(_apiResponse);
        }
    }
}