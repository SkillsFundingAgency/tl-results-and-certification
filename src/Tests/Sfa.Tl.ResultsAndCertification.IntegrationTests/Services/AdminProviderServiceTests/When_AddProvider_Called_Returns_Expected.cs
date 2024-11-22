using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests
{
    public class When_AddProvider_Called_Returns_Expected : AdminProviderServiceBaseTest
    {
        private const long Ukprn = 12345678;

        private IAdminProviderService _adminProviderService;
        private int _result;

        private readonly AddProviderRequest _request = new()
        {
            UkPrn = Ukprn,
            Name = "new-provider-name",
            DisplayName = "new-provider-display-name",
            CreatedBy = "created-by"
        };

        public override void Given()
        {
            _adminProviderService = CreateAdminProviderService();
        }

        public override async Task When()
        {
            _result = await _adminProviderService.AddProviderAsync(_request);
        }

        [Fact]
        public async void Then_Should_Return_False()
        {
            _result.Should().BeGreaterThan(0);

            TlProvider provider = await DbContext.TlProvider.SingleAsync(p => p.UkPrn == Ukprn);

            provider.UkPrn.Should().Be(Ukprn);
            provider.Name.Should().Be(_request.Name);
            provider.DisplayName.Should().Be(_request.DisplayName);
            provider.CreatedBy.Should().Be(_request.CreatedBy);
            provider.ModifiedBy.Should().BeNull();
            provider.ModifiedOn.Should().BeNull();
            provider.TlProviderAddresses.Should().BeEmpty();        
        }
    }
}