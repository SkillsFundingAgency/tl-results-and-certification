using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminReviewChangesAppealOutcommCoreTests
{
    public class When_Result_Exists : ProcessAdminReviewChangesAppealOutcomeSpecialismBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int TlLookupId = 1;
        private const int NewTlLookupId = 3;

        private TqPathwayResult _existingPathwayResult;
        private TqPathwayResult _newPathwayResult;


        private ReviewChangesAppealOutcomeCoreRequest _request;
        private IAdminPostResultsService _service;

        private bool _result;

        public override void Given()
        {
            _existingPathwayResult = CreateAndSavePathwayResult(RegistrationPathwayId, TlLookupId);
            _newPathwayResult = CreatePathwayResult(_existingPathwayResult.TqPathwayAssessmentId, NewTlLookupId);


            _service = CreateAdminPostResultsService();
            _request = CreateRequest(RegistrationPathwayId, _existingPathwayResult.Id);
        }

        public override async Task When()
        {
            _result = await _service.ProcessAdminReviewChangesAppealOutcomeCoreAsync(_request);
        }

        [Fact]
        public async void Then_Should_Return_False()
        {
            _result.Should().BeTrue();

            TqPathwayResult existingPathwayResultDb = await DbContext.TqPathwayResult.SingleAsync(ip => ip.Id == _existingPathwayResult.Id);
            existingPathwayResultDb.TqPathwayAssessmentId.Should().Be(_existingPathwayResult.TqPathwayAssessmentId);
            existingPathwayResultDb.TlLookupId.Should().Be(_existingPathwayResult.TlLookupId);
            existingPathwayResultDb.PrsStatus.Should().Be(_existingPathwayResult.PrsStatus);
            existingPathwayResultDb.IsOptedin.Should().BeFalse();
            existingPathwayResultDb.StartDate.Should().Be(_existingPathwayResult.StartDate);
            existingPathwayResultDb.EndDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            existingPathwayResultDb.IsBulkUpload.Should().Be(_existingPathwayResult.IsBulkUpload);
            existingPathwayResultDb.CreatedBy.Should().Be(_existingPathwayResult.CreatedBy);
            existingPathwayResultDb.CreatedOn.Should().Be(_existingPathwayResult.CreatedOn);
            existingPathwayResultDb.ModifiedBy.Should().Be(_request.CreatedBy);
            existingPathwayResultDb.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));

            TqPathwayResult newPathwayResultDb = await DbContext.TqPathwayResult.SingleAsync(ip => ip.TqPathwayAssessmentId == existingPathwayResultDb.TqPathwayAssessmentId && ip.IsOptedin);
            newPathwayResultDb.TqPathwayAssessmentId.Should().Be(_existingPathwayResult.TqPathwayAssessmentId);
            newPathwayResultDb.TlLookupId.Should().Be(_newPathwayResult.TlLookupId);
            newPathwayResultDb.PrsStatus.Should().Be(PrsStatus.Final);
            newPathwayResultDb.IsOptedin.Should().BeTrue();
            newPathwayResultDb.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            newPathwayResultDb.EndDate.Should().BeNull();
            newPathwayResultDb.IsBulkUpload.Should().BeFalse();
            newPathwayResultDb.CreatedBy.Should().Be(_request.CreatedBy);
            newPathwayResultDb.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            newPathwayResultDb.ModifiedBy.Should().BeNull();
            newPathwayResultDb.ModifiedOn.Should().NotHaveValue();

            ChangeLog changeLog = await DbContext.ChangeLog.SingleAsync(ip => ip.TqRegistrationPathwayId == _request.RegistrationPathwayId);
            changeLog.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            changeLog.ChangeType.Should().Be(ChangeType.PathwayAppealOutcome);
            changeLog.ReasonForChange.Should().Be(_request.ChangeReason);
            changeLog.DateOfRequest.Should().Be(_request.DateOfRequest);
            changeLog.Details.Should().Be(JsonConvert.SerializeObject(new
            {
                PathwayResultId = newPathwayResultDb.Id,
                From = _request.ExistingGrade,
                To = _request.SelectedGrade,
                PathwayAssessmentId = existingPathwayResultDb.TqPathwayAssessmentId,
                RegistrationPathwayId = _request.RegistrationPathwayId

            }));
            changeLog.ZendeskTicketID.Should().Be(_request.ZendeskTicketId);
            changeLog.Name.Should().Be(_request.ContactName);
            changeLog.CreatedBy.Should().Be(_request.CreatedBy);
        }
    }
}