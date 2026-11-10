using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.AddProviderTests
{
    public class When_Request_Duplicated_DisplayName_Returns_Expected : AddProviderBaseTest
    {
        public override void Given()
        {
            SeedTestData();

            Request = new()
            {
                UkPrn = 12345678,
                Name = "The new provider name",
                DisplayName = ProvidersDb.First().DisplayName,
                CreatedBy = "created-by"
            };
        }

        [Fact]
        public async Task Then_Should_Return_Unsuccessful_Response()
        {
            Result.ProviderId.Should().Be(0);
            Result.IsRequestValid.Should().BeFalse();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeFalse();
            Result.DuplicatedDisplayNameFound.Should().BeTrue();
            Result.Success.Should().BeFalse();

            bool exists = await DbContext.TlProvider.AnyAsync(p => p.UkPrn == Request.UkPrn, TestContext.Current.CancellationToken);
            exists.Should().BeFalse();
        }
    }
}