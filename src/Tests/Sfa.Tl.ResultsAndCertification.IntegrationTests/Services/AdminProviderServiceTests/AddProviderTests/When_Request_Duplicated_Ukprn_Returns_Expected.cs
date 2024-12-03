using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.AddProviderTests
{
    public class When_Request_Duplicated_Ukprn_Returns_Expected : AddProviderBaseTest
    {
        public override void Given()
        {
            SeedTestData();

            Request = new()
            {
                UkPrn = ProvidersDb.First().UkPrn,
                Name = "The new provider name",
                DisplayName = "The new provider display name",
                CreatedBy = "created-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_Unsuccessful_Response()
        {
            Result.ProviderId.Should().Be(0);
            Result.IsRequestValid.Should().BeFalse();
            Result.DuplicatedUkprnFound.Should().BeTrue();
            Result.DuplicatedNameFound.Should().BeFalse();
            Result.DuplicatedDisplayNameFound.Should().BeFalse();
            Result.Success.Should().BeFalse();

            bool exists = await DbContext.TlProvider.AnyAsync(p => p.Name == Request.Name);
            exists.Should().BeFalse();
        }
    }
}