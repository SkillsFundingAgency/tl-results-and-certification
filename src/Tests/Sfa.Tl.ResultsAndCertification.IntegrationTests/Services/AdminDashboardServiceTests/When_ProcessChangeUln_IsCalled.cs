using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public class When_ProcessChangeUln_IsCalled : AdminDashboardServiceBaseTest
    {
        public override void Given()
        {
            SeedTestData();
            CreateAdminDasboardService();
        }
        public override Task When() => Task.CompletedTask;

        [Theory]
        [InlineData(RegistrationPathwayStatus.Active)]
        [InlineData(RegistrationPathwayStatus.Withdrawn)]
        public async Task Then_profile_changes_and_attainment_and_audit_are_retained(RegistrationPathwayStatus status)
        {
            var profile = SeedRegistrationsData(new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, status } }, TqProvider).Single();
            var pathway = profile.TqRegistrationPathways.Single();
            pathway.AcademicYear = 2020;
            var assessment = GetPathwayAssessmentsDataToProcess(new() { pathway }).Single();
            var result = GetPathwayResultDataToProcess(assessment).Single();
            DbContext.SaveChanges();
            var profileId = profile.Id;
            var pathwayId = pathway.Id;
            var assessmentId = assessment.Id;
            var resultId = result.Id;
            var gradeId = result.TlLookupId;
            DbContext.ChangeTracker.Clear();

            var outcome = await AdminDashboardService.ProcessChangeUlnAsync(new ReviewChangeUlnRequest
            {
                RegistrationPathwayId = pathwayId, ContactName = "Contact", ChangeReason = "ULN correction",
                RequestDate = DateTime.Today, CreatedBy = "Admin",
                ChangeUlnDetails = new() { UlnFrom = 1111111111, UlnTo = 9999999991 }
            });
            Assert.True(outcome);
            DbContext.ChangeTracker.Clear();
            Assert.Equal(9999999991, (await DbContext.TqRegistrationProfile.FindAsync(profileId)).UniqueLearnerNumber);
            var savedPathway = await DbContext.TqRegistrationPathway.FindAsync(pathwayId);
            Assert.Equal(profileId, savedPathway.TqRegistrationProfileId);
            Assert.Equal(status, savedPathway.Status);
            Assert.Equal(2020, savedPathway.AcademicYear);
            Assert.Equal(pathwayId, (await DbContext.TqPathwayAssessment.FindAsync(assessmentId)).TqRegistrationPathwayId);
            Assert.Equal(gradeId, (await DbContext.TqPathwayResult.FindAsync(resultId)).TlLookupId);
            var log = await DbContext.ChangeLog.SingleAsync(l => l.TqRegistrationPathwayId == pathwayId && l.ChangeType == ChangeType.Uln);
            var details = JsonConvert.DeserializeObject<ChangeUlnDetails>(log.Details);
            Assert.Equal(1111111111, details.UlnFrom);
            Assert.Equal(9999999991, details.UlnTo);
        }
    }
}
