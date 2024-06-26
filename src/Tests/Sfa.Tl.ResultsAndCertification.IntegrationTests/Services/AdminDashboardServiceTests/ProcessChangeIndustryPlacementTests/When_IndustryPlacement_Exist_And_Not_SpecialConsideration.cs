﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests.ProcessChangeIndustryPlacementTests
{
    public class When_IndustryPlacement_Exist_And_Not_SpecialConsideration : ProcessChangeIndustryPlacementTestsBase
    {
        private const int RegistrationPathwayId = 1;

        private ReviewChangeIndustryPlacementRequest _request;
        private bool _result;

        private int _industryPlacementId;

        public override void Given()
        {
            _industryPlacementId = CreateAndSaveIndustryPlacement(RegistrationPathwayId, IndustryPlacementStatus.NotCompleted);
            _request = CreateRequest(RegistrationPathwayId, IndustryPlacementStatus.Completed);

            CreateAdminDasboardService();
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessChangeIndustryPlacementAsync(_request);
        }

        [Fact]
        public async void Then_Should_Save_Expected_And_Return_True()
        {
            _result.Should().BeTrue();

            IndustryPlacement industryPlacement = await DbContext.IndustryPlacement.SingleAsync(p => p.Id == _industryPlacementId);
            industryPlacement.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            industryPlacement.Status.Should().Be(_request.IndustryPlacementStatus);
            industryPlacement.Details.Should().BeNull();

            ChangeLog changeLog = await DbContext.ChangeLog.SingleAsync(p => p.TqRegistrationPathwayId == _request.RegistrationPathwayId && p.ChangeType == _request.ChangeType);
            changeLog.TqRegistrationPathwayId.Should().Be(_request.RegistrationPathwayId);
            changeLog.ChangeType.Should().Be(_request.ChangeType);
            changeLog.ReasonForChange.Should().Be(_request.ChangeReason);
            changeLog.DateOfRequest.Should().Be(_request.RequestDate);
            changeLog.ZendeskTicketID.Should().Be(_request.ZendeskId);
            changeLog.Name.Should().Be(_request.ContactName);
            changeLog.CreatedBy.Should().Be(_request.CreatedBy);

            var changeLogDetails = new ChangeIndustryPlacementRequest
            {
                IndustryPlacementStatusFrom = IndustryPlacementStatus.NotCompleted,
                IndustryPlacementStatusTo = _request.IndustryPlacementStatus,
                HoursSpentOnPlacementFrom = null,
                HoursSpentOnPlacementTo = null,
                SpecialConsiderationReasonsFrom = null,
                SpecialConsiderationReasonsTo = null
            };

            changeLog.Details.Should().Be(JsonConvert.SerializeObject(changeLogDetails));
        }
    }
}