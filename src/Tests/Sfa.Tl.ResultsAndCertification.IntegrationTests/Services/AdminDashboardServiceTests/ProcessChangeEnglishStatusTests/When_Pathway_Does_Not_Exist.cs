using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeEnglishStatusTests
{
    public class When_Pathway_Does_Not_Exist : ProcessChangeEnglishStatusTestsBase
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
                     c.ChangeType == ChangeType.EnglishStatus);

            changeLog.Should().BeNull();
        }
    }
}