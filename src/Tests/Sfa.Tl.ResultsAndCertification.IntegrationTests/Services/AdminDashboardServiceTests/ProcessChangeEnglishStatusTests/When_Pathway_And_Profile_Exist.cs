using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeEnglishStatusTests
{
    public class When_Pathway_And_Profile_Exist : ProcessChangeEnglishStatusTestsBase
    {
        private const SubjectStatus OriginalEnglishStatus = SubjectStatus.NotAchieved;
        private const SubjectStatus NewEnglishStatus = SubjectStatus.Achieved;

        private int _registrationPathwayId;
        private long _uln;

        public override void Given()
        {
            (_registrationPathwayId, _uln) = CreateAndSaveProfileWithEnglishStatus(OriginalEnglishStatus);

            Request = CreateRequest(_registrationPathwayId, NewEnglishStatus);

            CreateAdminDasboardService();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            Result.Should().BeTrue();
        }

        [Fact]
        public async Task Then_Should_Update_English_Status()
        {
            var profile = await DbContext.TqRegistrationProfile.SingleAsync(p => p.UniqueLearnerNumber == _uln);
            profile.EnglishStatus.Should().Be(NewEnglishStatus);
        }

        [Fact]
        public async Task Then_Should_Create_Change_Log()
        {
            var changeLog = await DbContext.ChangeLog.SingleOrDefaultAsync(
                c => c.TqRegistrationPathwayId == _registrationPathwayId &&
                     c.ChangeType == ChangeType.SubjectStatus);

            changeLog.Should().NotBeNull();
            changeLog.ReasonForChange.Should().Be(Request.ChangeReason);
            changeLog.ZendeskTicketID.Should().Be(Request.ZendeskId);
            changeLog.Name.Should().Be(Request.ContactName);
            changeLog.CreatedBy.Should().Be(Request.CreatedBy);

            var details = JsonConvert.DeserializeObject<dynamic>(changeLog.Details);
            ((string)details.EnglishStatusFrom).Should().Be(OriginalEnglishStatus.ToString());
            ((string)details.EnglishStatusTo).Should().Be(NewEnglishStatus.ToString());
        }
    }
}