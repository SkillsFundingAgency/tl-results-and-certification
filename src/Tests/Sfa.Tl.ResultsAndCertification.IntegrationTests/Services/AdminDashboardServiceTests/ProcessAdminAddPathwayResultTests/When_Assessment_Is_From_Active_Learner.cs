using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessAdminAddPathwayResultTests
{
    public class When_Assessment_Is_From_Active_Learner : ProcessAdminAddPathwayResultTestsBase
    {
        private const int RegistrationPathwayId = 1;

        private AddPathwayResultRequest _request;
        private bool _result;

        public override void Given()
        {
            int pathwayAssessmentId = CreateAndSavePathwayAssessment(RegistrationPathwayId);
            _request = CreateRequest(RegistrationPathwayId, pathwayAssessmentId);

            CreateAdminDasboardService();
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddPathwayResultAsync(_request);
        }

        [Fact]
        public async Task Then_Should_Return_True()
        {
            _result.Should().BeTrue();

            TqPathwayResult pathwayResult = await DbContext.TqPathwayResult.SingleAsync(ip => ip.TqPathwayAssessmentId == _request.PathwayAssessmentId);
            pathwayResult.TqPathwayAssessmentId.Should().Be(_request.PathwayAssessmentId);
            pathwayResult.TlLookupId.Should().Be(_request.SelectedGradeId);
            pathwayResult.IsOptedin.Should().BeTrue();
            pathwayResult.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            pathwayResult.EndDate.Should().BeNull();
            pathwayResult.IsBulkUpload.Should().BeFalse();
            pathwayResult.CreatedBy.Should().Be(_request.CreatedBy);
            pathwayResult.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));

            ChangeLog changeLog = await DbContext.ChangeLog.SingleAsync(ip => ip.TqRegistrationPathwayId == _request.RegistrationPathwayId);
            changeLog.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            changeLog.ChangeType.Should().Be(_request.ChangeType);
            changeLog.ReasonForChange.Should().Be(_request.ChangeReason);
            changeLog.DateOfRequest.Should().Be(_request.RequestDate);
            changeLog.Details.Should().Be(JsonConvert.SerializeObject(new { PathwayResultId = pathwayResult.Id, PathwayAssessmentId = pathwayResult.TqPathwayAssessmentId, _request.GradeTo }));
            changeLog.ZendeskTicketID.Should().Be(_request.ZendeskId);
            changeLog.Name.Should().Be(_request.ContactName);
            changeLog.CreatedBy.Should().Be(_request.CreatedBy);
        }
    }
}