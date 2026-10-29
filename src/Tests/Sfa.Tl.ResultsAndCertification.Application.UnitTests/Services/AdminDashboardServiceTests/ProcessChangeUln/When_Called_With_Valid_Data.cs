using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessChangeUln
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            base.Given();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            Result.Should().BeTrue();
            Profile.UniqueLearnerNumber.Should().Be(9999999991);
            await Logs.Received(1).CreateAsync(Arg.Is<ChangeLog>(l => l.ChangeType == ChangeType.Uln
                && l.TqRegistrationPathwayId == 3 && l.CreatedBy == "Admin"
                && l.Details.Contains("9999999990") && l.Details.Contains("9999999991")));
            await Pathways.DidNotReceive().UpdateAsync(Arg.Any<TqRegistrationPathway>());
        }
    }
}
