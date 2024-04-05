using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResultTests
{
    public class When_Assessment_Is_From_Active_Learner : ProcessAdminAddSpecialismResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int RegistrationSpecialismId = 1;

        private AddSpecialismResultRequest _request;
        private bool _result;

        public override void Given()
        {
            int specialismAssessmentId = CreateAndSaveSpecialismAssessment(RegistrationSpecialismId);
            _request = CreateRequest(RegistrationPathwayId, specialismAssessmentId);

            CreateAdminDasboardService();
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddSpecialismResultAsync(_request);
        }

        [Fact]
        public async Task Then_Should_Return_True()
        {
            _result.Should().BeTrue();

            TqSpecialismResult specialismResult = await DbContext.TqSpecialismResult.SingleAsync(ip => ip.TqSpecialismAssessmentId == _request.SpecialismAssessmentId);
            specialismResult.TqSpecialismAssessmentId.Should().Be(_request.SpecialismAssessmentId);
            specialismResult.TlLookupId.Should().Be(_request.SelectedGradeId);
            specialismResult.IsOptedin.Should().BeTrue();
            specialismResult.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            specialismResult.EndDate.Should().BeNull();
            specialismResult.IsBulkUpload.Should().BeFalse();
            specialismResult.CreatedBy.Should().Be(_request.CreatedBy);
            specialismResult.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));

            ChangeLog changeLog = await DbContext.ChangeLog.SingleAsync(ip => ip.TqRegistrationPathwayId == _request.RegistrationPathwayId);
            changeLog.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            changeLog.ChangeType.Should().Be(_request.ChangeType);
            changeLog.ReasonForChange.Should().Be(_request.ChangeReason);
            changeLog.DateOfRequest.Should().Be(_request.RequestDate);
            changeLog.Details.Should().Be(JsonConvert.SerializeObject(new { SpecialismResultId = specialismResult.Id, GradeTo = _request.GradeTo }));
            changeLog.ZendeskTicketID.Should().Be(_request.ZendeskId);
            changeLog.Name.Should().Be(_request.ContactName);
            changeLog.CreatedBy.Should().Be(_request.CreatedBy);
        }
    }
}