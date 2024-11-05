using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.UpdateProviderTests
{
    public class When_UpdateProvider_Request_Valid_Returns_Expected : UpdateProviderBaseTest
    {
        private const long Ukprn = 12345678;
        private TlProvider _providerDb;

        public override void Given()
        {
            int providerId = SeedTestData();

            Request = new()
            {
                ProviderId = providerId,
                UkPrn = Ukprn,
                Name = "Barnsley College",
                DisplayName = "Barnsley College",
                IsActive = true,
                ModifiedBy = "modified-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_False()
        {
            Result.IsRequestValid.Should().BeTrue();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeFalse();
            Result.Success.Should().BeTrue();

            TlProvider provider = await DbContext.TlProvider.SingleAsync(p => p.Id == Request.ProviderId);

            provider.Id.Should().Be(_providerDb.Id);
            provider.UkPrn.Should().Be(Request.UkPrn);
            provider.Name.Should().Be(Request.Name);
            provider.DisplayName.Should().Be(Request.DisplayName);
            provider.IsActive.Should().Be(Request.IsActive);
            provider.CreatedBy.Should().Be(_providerDb.CreatedBy);
            provider.CreatedOn.Should().Be(_providerDb.CreatedOn);
            provider.ModifiedBy.Should().Be(Request.ModifiedBy);
            provider.ModifiedOn.Should().Be(Now);
        }

        private int SeedTestData()
        {
            TlProviderBuilder providerBuilder = new();
            _providerDb = providerBuilder.Build();

            DbContext.Add(_providerDb);
            DbContext.SaveChanges();

            return _providerDb.Id;
        }
    }
}