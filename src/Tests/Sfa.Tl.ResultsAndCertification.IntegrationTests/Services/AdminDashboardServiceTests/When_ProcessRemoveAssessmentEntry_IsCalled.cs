﻿using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public class When_ProcessRemoveAssessmentEntry_IsCalled : AdminDashboardServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private ReviewRemoveCoreAssessmentEntryRequest ReviewRemoveAssessmentEntryRequest;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;

        private bool _result;
        private int AssessmentId;
        private int RegistrationPathwayId;

        public override void Given()
        {
            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                //Build Pathway results
                var ulnWithResult = new List<long> { 1111111114, 1111111115 };
                if (ulnWithResult.Any(x => x == registration.UniqueLearnerNumber))
                {
                    foreach (var assessment in pathwayAssessments)
                    {
                        var hasHitoricResult = new List<long> { 1111111115 };
                        var isHistoricResult = hasHitoricResult.Any(x => x == registration.UniqueLearnerNumber);
                        var isLatestActiveResult = !isHistoricResult;

                        var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, isLatestActiveResult, isHistoricResult);
                        tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                    }
                }
            }
            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            SeedPathwayResultsData(tqPathwayResultsSeedData);

            CreateAdminDasboardService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            var assessment = DbContext.TqPathwayAssessment.FirstOrDefault(s => s.IsOptedin && s.EndDate == null && s.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active);

            AssessmentId = assessment.Id;
            RegistrationPathwayId = assessment.TqRegistrationPathwayId;

            ReviewRemoveAssessmentEntryRequest = new ReviewRemoveCoreAssessmentEntryRequest
            {
                AssessmentId = AssessmentId,
                ComponentType = ComponentType.Core,
                RegistrationPathwayId = RegistrationPathwayId,
                CreatedBy = "Admin User",
                ChangeReason = "Change Reason",
                ContactName = "Test User",
                RequestDate = SystemProvider.Today,
                ZendeskId = "123456789",
                ChangeAssessmentDetails = new()
                {
                    From = "Autumn 2023",
                    To = string.Format(AdminReviewRemoveAssessmentEntry.Label_No_Assessment_Entry_Recorded, "Autumn 2022"),
                    PathwayName = "Design, Surveying and Planning (10123456)"
                }
            };

            _result = await AdminDashboardService.ProcessRemovePathwayAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();
            _result.Should().Be(true);

            var actualPathwayAssessment = DbContext.TqPathwayAssessment.SingleOrDefault(s => s.Id == ReviewRemoveAssessmentEntryRequest.AssessmentId);
            var actualChangeLog = DbContext.ChangeLog.SingleOrDefault(s => s.TqRegistrationPathwayId == ReviewRemoveAssessmentEntryRequest.RegistrationPathwayId);
            var actualChangeDetails = JsonConvert.DeserializeObject<DetailsChangeAssessmentRemove>(actualChangeLog.Details);

            actualPathwayAssessment.Should().NotBeNull();
            actualPathwayAssessment.IsOptedin.Should().BeFalse();
            actualPathwayAssessment.EndDate.Should().NotBeNull();
            actualPathwayAssessment.ModifiedBy.Should().NotBeNull();
            actualPathwayAssessment.ModifiedOn.Should().NotBeNull();

            actualChangeLog.Should().NotBeNull();
            actualChangeLog.ChangeType.Should().Be(ReviewRemoveAssessmentEntryRequest.ChangeType);
            actualChangeLog.Name.Should().Be(ReviewRemoveAssessmentEntryRequest.ContactName);
            actualChangeLog.CreatedBy.Should().Be(ReviewRemoveAssessmentEntryRequest.CreatedBy);
            actualChangeLog.ReasonForChange.Should().Be(ReviewRemoveAssessmentEntryRequest.ChangeReason);
            actualChangeLog.DateOfRequest.Should().Be(ReviewRemoveAssessmentEntryRequest.RequestDate);
            actualChangeLog.ZendeskTicketID.Should().Be(ReviewRemoveAssessmentEntryRequest.ZendeskId);

            actualChangeDetails.Should().NotBeNull();
            actualChangeDetails.From.Should().Be(ReviewRemoveAssessmentEntryRequest.ChangeAssessmentDetails.From);
            actualChangeDetails.To.Should().Be(ReviewRemoveAssessmentEntryRequest.ChangeAssessmentDetails.To);
            actualChangeDetails.PathwayName.Should().Be(ReviewRemoveAssessmentEntryRequest.ChangeAssessmentDetails.PathwayName);
        }
    }
}
