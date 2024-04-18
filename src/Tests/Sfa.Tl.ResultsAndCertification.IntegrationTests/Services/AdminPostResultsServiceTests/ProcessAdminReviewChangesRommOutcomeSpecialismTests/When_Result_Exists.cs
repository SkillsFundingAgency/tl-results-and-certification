using Azure.Core;
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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests.ProcessAdminReviewChangesRommOutcommSpecialismTests
{
    public class When_Result_Exists : ProcessAdminReviewChangesRommOutcomeSpecialismBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int TlLookupId = 12;
        private const int NewTlLookupId = 11;

        private TqSpecialismResult _existingSpecialismResult;
        private TqSpecialismResult _newSpecialismResult;


        private ReviewChangesRommOutcomeSpecialismRequest _request;
        private IAdminPostResultsService _service;

        private bool _result;

        public override void Given()
        {
            _existingSpecialismResult = CreateAndSaveSpecialismResult(RegistrationPathwayId, TlLookupId);
            _newSpecialismResult = CreateSpecialismResult(_existingSpecialismResult.TqSpecialismAssessmentId, NewTlLookupId);


            _service = CreateAdminPostResultsService();
            _request = CreateRequest(RegistrationPathwayId, _existingSpecialismResult.Id);
        }

        public override async Task When()
        {
            _result = await _service.ProcessAdminReviewChangesRommOutcomeSpecialismAsync(_request);
        }

        [Fact]
        public async void Then_Should_Return_False()
        {
            _result.Should().BeTrue();

            TqSpecialismResult existingSpecialismResultDb = await DbContext.TqSpecialismResult.SingleAsync(ip => ip.Id == _existingSpecialismResult.Id);
            existingSpecialismResultDb.TqSpecialismAssessmentId.Should().Be(_existingSpecialismResult.TqSpecialismAssessmentId);
            existingSpecialismResultDb.TlLookupId.Should().Be(_existingSpecialismResult.TlLookupId);
            existingSpecialismResultDb.PrsStatus.Should().Be(_existingSpecialismResult.PrsStatus);
            existingSpecialismResultDb.IsOptedin.Should().BeFalse();
            existingSpecialismResultDb.StartDate.Should().Be(_existingSpecialismResult.StartDate);
            existingSpecialismResultDb.EndDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            existingSpecialismResultDb.IsBulkUpload.Should().Be(_existingSpecialismResult.IsBulkUpload);
            existingSpecialismResultDb.CreatedBy.Should().Be(_existingSpecialismResult.CreatedBy);
            existingSpecialismResultDb.CreatedOn.Should().Be(_existingSpecialismResult.CreatedOn);
            existingSpecialismResultDb.ModifiedBy.Should().Be(_request.CreatedBy);
            existingSpecialismResultDb.ModifiedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));

            TqSpecialismResult newSpecialismResultDb = await DbContext.TqSpecialismResult.SingleAsync(ip => ip.TqSpecialismAssessmentId == existingSpecialismResultDb.TqSpecialismAssessmentId && ip.IsOptedin);
            newSpecialismResultDb.TqSpecialismAssessmentId.Should().Be(_existingSpecialismResult.TqSpecialismAssessmentId);
            newSpecialismResultDb.TlLookupId.Should().Be(_newSpecialismResult.TlLookupId);
            newSpecialismResultDb.PrsStatus.Should().Be(PrsStatus.Reviewed);
            newSpecialismResultDb.IsOptedin.Should().BeTrue();
            newSpecialismResultDb.StartDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            newSpecialismResultDb.EndDate.Should().BeNull();
            newSpecialismResultDb.IsBulkUpload.Should().BeFalse();
            newSpecialismResultDb.CreatedBy.Should().Be(_request.CreatedBy);
            newSpecialismResultDb.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(15));
            newSpecialismResultDb.ModifiedBy.Should().BeNull();
            newSpecialismResultDb.ModifiedOn.Should().NotHaveValue();

            ChangeLog changeLog = await DbContext.ChangeLog.SingleAsync(ip => ip.TqRegistrationPathwayId == _request.RegistrationPathwayId);
            changeLog.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            changeLog.ChangeType.Should().Be(ChangeType.SpecialismRommOutcome);
            changeLog.ReasonForChange.Should().Be(_request.ChangeReason);
            changeLog.DateOfRequest.Should().Be(_request.DateOfRequest);
            changeLog.Details.Should().Be(JsonConvert.SerializeObject(new { 
                SpecialismResultId = newSpecialismResultDb.Id,
                From   = _request.ExistingGrade,
                To = _request.SelectedGrade,
                SpecialismAssessmentId = existingSpecialismResultDb.TqSpecialismAssessmentId,
                RegistrationPathwayId = _request.RegistrationPathwayId

            }));
            changeLog.ZendeskTicketID.Should().Be(_request.ZendeskTicketId);
            changeLog.Name.Should().Be(_request.ContactName);
            changeLog.CreatedBy.Should().Be(_request.CreatedBy);
        }
    }
}