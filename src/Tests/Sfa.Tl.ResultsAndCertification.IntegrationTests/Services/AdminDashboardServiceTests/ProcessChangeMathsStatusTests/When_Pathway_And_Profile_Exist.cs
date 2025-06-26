using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeMathsStatusTests
{
    public class When_Pathway_And_Profile_Exist : ProcessChangeMathsStatusTestsBase
    {
        private const SubjectStatus OriginalMathsStatus = SubjectStatus.NotAchieved;
        private const SubjectStatus NewMathsStatus = SubjectStatus.Achieved;

        private int _registrationPathwayId;
        private long _uln;

        public override void Given()
        {
            (_registrationPathwayId, _uln) = CreateAndSaveProfileWithMathsStatus(OriginalMathsStatus);

            Request = CreateRequest(_registrationPathwayId, NewMathsStatus);

            CreateAdminDasboardService();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            Result.Should().BeTrue();
        }

        [Fact]
        public async Task Then_Should_Update_Maths_Status()
        {
            var profile = await DbContext.TqRegistrationProfile.SingleAsync(p => p.UniqueLearnerNumber == _uln);
            profile.MathsStatus.Should().Be(NewMathsStatus);
        }

        [Fact]
        public async Task Then_Should_Create_Change_Log()
        {
            var changeLog = await DbContext.ChangeLog.SingleOrDefaultAsync(
                c => c.TqRegistrationPathwayId == _registrationPathwayId &&
                     c.ChangeType == ChangeType.MathsStatus);

            changeLog.Should().NotBeNull();
            changeLog.ReasonForChange.Should().Be(Request.ChangeReason);
            changeLog.ZendeskTicketID.Should().Be(Request.ZendeskId);
            changeLog.Name.Should().Be(Request.ContactName);
            changeLog.CreatedBy.Should().Be(Request.CreatedBy);

            var details = JsonConvert.DeserializeObject<dynamic>(changeLog.Details);
            ((int?)details.MathsStatusFrom).Should().Be((int?)OriginalMathsStatus);
            ((int?)details.MathsStatusTo).Should().Be((int?)NewMathsStatus);
        }
    }
}