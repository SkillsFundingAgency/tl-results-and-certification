using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.AddProviderTests
{
    public class When_Request_Duplicated_Name_Returns_Expected : AddProviderBaseTest
    {
        public override void Given()
        {
            SeedTestData();

            Request = new()
            {
                UkPrn = 12345678,
                Name = ProvidersDb.First().Name,
                DisplayName = "The new provider display name",
                CreatedBy = "created-by"
            };
        }

        [Fact]
        public async void Then_Should_Return_Unsuccessful_Response()
        {
            Result.ProviderId.Should().Be(0);
            Result.IsRequestValid.Should().BeFalse();
            Result.DuplicatedUkprnFound.Should().BeFalse();
            Result.DuplicatedNameFound.Should().BeTrue();
            Result.DuplicatedDisplayNameFound.Should().BeFalse();
            Result.Success.Should().BeFalse();

            bool exists = await DbContext.TlProvider.AnyAsync(p => p.UkPrn == Request.UkPrn);
            exists.Should().BeFalse();
        }
    }
}