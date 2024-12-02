using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.UpdateProviderTests
{
    public class When_Request_Duplicated_Name_Returns_Expected : UpdateProviderBaseTest
    {        
        public override void Given()
        {
            TlProvider provider = SeedTestData();

            Request = new()
            {
                ProviderId = provider.Id,
                UkPrn = provider.UkPrn,
                Name = "Walsall Studio School",
                DisplayName = provider.DisplayName,
                IsActive = provider.IsActive,
                ModifiedBy = "modified-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_Unsuccessful_Response()
        {
            Result.IsRequestValid.Should().BeFalse();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeTrue();
            Result.DuplicatedDisplayNameFound.Should().BeFalse();
            Result.Success.Should().BeFalse();

            TlProvider provider = await DbContext.TlProvider.SingleAsync(p => p.Id == Request.ProviderId);
            provider.Should().BeEquivalentTo(ProvidersDb.First());
        }
    }
}