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
    public class When_SubmitUpdateProviderRequest_Called_Returns_Expected : AdminProviderLoaderBaseTest
    {
        private readonly AdminEditProviderViewModel _request = new()
        {
            ProviderId = 1,
            UkPrn = "12345678",
            Name = "updated-name",
            DisplayName = "updated-display-name",
            IsActive = true
        };

        private readonly Expression<Predicate<UpdateProviderRequest>> _apiRequestPredicate =
            r => r.ProviderId == 1
            && r.UkPrn == 12345678
            && r.Name == "updated-name"
            && r.DisplayName == "updated-display-name"
            && r.IsActive == true
            && r.ModifiedBy == "test user";

        private readonly UpdateProviderResponse _apiResponse = new()
        {
            DuplicatedUkprnFound = false,
            DuplicatedNameFound = false,
            Success = true
        };

        private UpdateProviderResponse _result;

        public override void Given()
        {
            ApiClient
                .UpdateProviderAsync(Arg.Is(_apiRequestPredicate))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SubmitUpdateProviderRequest(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).UpdateProviderAsync(Arg.Is(_apiRequestPredicate));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeEquivalentTo(_apiResponse);
        }
    }
}