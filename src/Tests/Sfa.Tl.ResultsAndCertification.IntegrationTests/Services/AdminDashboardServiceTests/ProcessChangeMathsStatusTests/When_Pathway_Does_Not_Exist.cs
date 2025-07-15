using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeMathsStatusTests
{
    public class When_Pathway_Does_Not_Exist : ProcessChangeMathsStatusTestsBase
    {
        private const int NonExistentPathwayId = 9999;

        public override void Given()
        {
            Request = CreateRequest(NonExistentPathwayId);

            CreateAdminDasboardService();
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            Result.Should().BeFalse();
        }

        [Fact]
        public async Task Then_Should_Not_Create_Change_Log()
        {
            var changeLog = await DbContext.ChangeLog.SingleOrDefaultAsync(
                c => c.TqRegistrationPathwayId == NonExistentPathwayId &&
                     c.ChangeType == ChangeType.MathsStatus);

            changeLog.Should().BeNull();
        }
    }
}