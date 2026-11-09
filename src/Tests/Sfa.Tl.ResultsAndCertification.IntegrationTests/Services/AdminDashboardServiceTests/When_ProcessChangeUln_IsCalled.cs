using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
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
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;
        private bool _actualResult;

        public override void Given()
        {
            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
            };

            // Registrations seed
            SeedTestData();
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            var pathways = _registrations.SelectMany(x => x.TqRegistrationPathways).ToList();
            foreach (var pathway in pathways)
            {
                pathway.AcademicYear = 2020;
            }

            _pathwayAssessments = GetPathwayAssessmentsDataToProcess(pathways);
            _pathwayResults = _pathwayAssessments.SelectMany(x => GetPathwayResultDataToProcess(x)).ToList();

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            CreateAdminDasboardService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ReviewChangeUlnRequest request)
        {
            _actualResult = await AdminDashboardService.ProcessChangeUlnAsync(request);
        }

        [Theory]
        [InlineData(1111111111L)]
        [InlineData(1111111112L)]
        public async Task Then_Expected_Results_Are_Returned(long uln)
        {
            TqRegistrationProfile expected = _registrations.Single(x => x.UniqueLearnerNumber == uln);
            TqRegistrationPathway expectedPathway = expected.TqRegistrationPathways.Single();
            TqPathwayAssessment expectedAssessment = _pathwayAssessments.Single(x => x.TqRegistrationPathwayId == expectedPathway.Id);
            TqPathwayResult expectedResult = _pathwayResults.Single(x => x.TqPathwayAssessmentId == expectedAssessment.Id);

            ReviewChangeUlnRequest request = new()
            {
                RegistrationPathwayId = expectedPathway.Id,
                ChangeReason = "Test Reason",
                ContactName = "Test User",
                ChangeUlnDetails = new ChangeUlnDetails { UlnFrom = uln, UlnTo = 9999999991 },
                RequestDate = DateTime.Today,
                ZendeskId = "1234567890",
                CreatedBy = "System"
            };

            await WhenAsync(request);

            DbContext.ChangeTracker.Clear();
            TqRegistrationProfile actual = await DbContext.TqRegistrationProfile.SingleAsync(x => x.Id == expected.Id);
            TqRegistrationPathway actualPathway = await DbContext.TqRegistrationPathway.SingleAsync(x => x.Id == request.RegistrationPathwayId);
            TqPathwayAssessment actualAssessment = await DbContext.TqPathwayAssessment.SingleAsync(x => x.Id == expectedAssessment.Id);
            TqPathwayResult actualResult = await DbContext.TqPathwayResult.SingleAsync(x => x.Id == expectedResult.Id);
            ChangeLog actualChangeLog = await DbContext.ChangeLog.SingleAsync(x => x.TqRegistrationPathwayId == request.RegistrationPathwayId && x.ChangeType == ChangeType.Uln);
            ChangeUlnDetails actualDetails = JsonConvert.DeserializeObject<ChangeUlnDetails>(actualChangeLog.Details);

            // Assert
            _actualResult.Should().BeTrue();
            actual.UniqueLearnerNumber.Should().Be(request.ChangeUlnDetails.UlnTo);
            actual.ModifiedBy.Should().Be(request.CreatedBy);
            actual.ModifiedOn.Should().HaveValue();
            actualPathway.TqRegistrationProfileId.Should().Be(expected.Id);
            actualPathway.Status.Should().Be(expectedPathway.Status);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            actualAssessment.TqRegistrationPathwayId.Should().Be(expectedPathway.Id);
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualDetails.UlnFrom.Should().Be(request.ChangeUlnDetails.UlnFrom);
            actualDetails.UlnTo.Should().Be(request.ChangeUlnDetails.UlnTo);
        }
    }
}
