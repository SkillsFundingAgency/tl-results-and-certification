using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.AddProviderTests
{
    public class When_Request_Valid_Returns_Expected : AddProviderBaseTest
    {
        private const long Ukprn = 12345678;

        public override void Given()
        {
            SeedTestData();

            Request = new()
            {
                UkPrn = Ukprn,
                Name = "new-provider-name",
                DisplayName = "new-provider-display-name",
                CreatedBy = "created-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_Successful_Response()
        {
            Result.ProviderId.Should().Be(ProvidersDb.Max(p => p.Id) + 1);
            Result.IsRequestValid.Should().BeTrue();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeFalse();
            Result.Success.Should().BeTrue();

            TlProvider provider = await DbContext.TlProvider.SingleAsync(p => p.UkPrn == Ukprn);

            provider.UkPrn.Should().Be(Ukprn);
            provider.Name.Should().Be(Request.Name);
            provider.DisplayName.Should().Be(Request.DisplayName);
            provider.CreatedBy.Should().Be(Request.CreatedBy);
            provider.ModifiedBy.Should().BeNull();
            provider.ModifiedOn.Should().BeNull();
            provider.TlProviderAddresses.Should().BeEmpty();
        }
    }
}