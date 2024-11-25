using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.UpdateProviderTests
{
    public class When_Request_Valid_Returns_Expected : UpdateProviderBaseTest
    {
        public override void Given()
        {
            TlProvider provider = SeedTestData();

            Request = new()
            {
                ProviderId = provider.Id,
                UkPrn = 12345678,
                Name = "The new provider name",
                DisplayName = "The new provider display name",
                IsActive = true,
                ModifiedBy = "modified-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_Successful_Response()
        {
            Result.IsRequestValid.Should().BeTrue();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeFalse();
            Result.Success.Should().BeTrue();

            TlProvider providerDb = ProvidersDb.First();
            TlProvider provider = await DbContext.TlProvider.SingleAsync(p => p.Id == Request.ProviderId);

            provider.Id.Should().Be(providerDb.Id);
            provider.UkPrn.Should().Be(Request.UkPrn);
            provider.Name.Should().Be(Request.Name);
            provider.DisplayName.Should().Be(Request.DisplayName);
            provider.IsActive.Should().Be(Request.IsActive);
            provider.CreatedBy.Should().Be(providerDb.CreatedBy);
            provider.CreatedOn.Should().Be(providerDb.CreatedOn);
            provider.ModifiedBy.Should().Be(Request.ModifiedBy);
            provider.ModifiedOn.Should().Be(Now);
        }
    }
}