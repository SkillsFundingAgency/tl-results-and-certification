using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.ProcessChangeUln
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            Result.Should().BeTrue();

            await ApiClient.Received(1).ProcessChangeUlnAsync(Arg.Is<ReviewChangeUlnRequest>(r =>
                r.RegistrationPathwayId == 3 && r.ChangeUlnDetails.UlnFrom == 9999999990
                && r.ChangeUlnDetails.UlnTo == 9999999991 && r.ContactName == "Contact"
                && r.ChangeReason == "Correction" && r.ZendeskId == "123"
                && r.RequestDate == new DateTime(2026, 2, 1) && r.CreatedBy == "test user"));
        }
    }
}
