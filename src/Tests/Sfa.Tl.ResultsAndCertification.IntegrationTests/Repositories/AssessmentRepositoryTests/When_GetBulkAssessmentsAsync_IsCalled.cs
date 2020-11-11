using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public class When_GetBulkAssessmentsAsync_IsCalled : AssessmentRepositoryBaseTest
    {
        private long _aoUkprn;
        private Dictionary<long, RegistrationPathwayStatus> _ulns;

        private IEnumerable<TqRegistrationPathway> _result;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;

        public override void Given()
        {
            // Parameters
            _aoUkprn = 10011881;
            //_ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn }, { 1111111114, RegistrationPathwayStatus.Active } };

            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            var registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var isHitoricalRec = new List<long> { 1111111112 };
                var isHistoricAssessent = isHitoricalRec.Any(x => x == registration.UniqueLearnerNumber);
                var seedActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), !isHistoricAssessent && seedActive, isHistoricAssessent));
                
                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), !isHistoricAssessent && seedActive, isHistoricAssessent));
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            DbContext.SaveChanges();
            
            // TestClass
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_result != null)
                return;

            _result = await AssessmentRepository.GetBulkAssessmentsAsync(_aoUkprn, _ulns.Keys);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            // when
            await WhenAsync();

            _result.Should().NotBeNullOrEmpty();
            _result.Count().Should().Be(_ulns.Count);

            // Uln: 1111111111 - Registration(Active) with no assessments
            var actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111111);
            actualresult.Should().NotBeNull();
            actualresult.TqPathwayAssessments.Should().BeEmpty();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Should().BeEmpty();

            // Uln: 1111111113 - Registration(Withdrawn), TqPathwayAssessments(Withdrawn) and TqSpecialismAssessments(Withdrawn)
            actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111113);
            actualresult.Should().NotBeNull();
            actualresult.TqPathwayAssessments.Should().BeEmpty();
            actualresult.TqRegistrationSpecialisms.Should().BeEmpty();

            // Uln: 1111111112 - Registration(Active), TqPathwayAssessments(Active + History) and TqSpecialismAssessments(Active + History)
            actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111112);
            actualresult.Should().NotBeNull();
            actualresult.TqPathwayAssessments.Should().HaveCount(1);
            actualresult.TqRegistrationSpecialisms.Should().HaveCount(1);
            
            // Assert PathwayAssessment
            var actualPathwayAssessment = actualresult.TqPathwayAssessments.FirstOrDefault();
            var expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == 1111111112 && x.IsOptedin && x.EndDate == null);
            AssertPathwayAssessment(actualPathwayAssessment, expectedPathwayAssessment);

            var actualSpecialismAssessment = actualresult.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.FirstOrDefault();
            var expectedSpecialismAssessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == 1111111112 && x.IsOptedin && x.EndDate == null);
            AssertSpecialismAssessment(actualSpecialismAssessment, expectedSpecialismAssessment);

            // 1   Active           Exists
            // 2   Active(History)  Exists(History)    -- TODO.
        }

        private void AssertSpecialismAssessment(TqSpecialismAssessment actualSpecialismAssessment, TqSpecialismAssessment expectedSpecialismAssessment)
        {
            actualSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedSpecialismAssessment.TqRegistrationSpecialismId);
            actualSpecialismAssessment.AssessmentSeriesId.Should().Be(expectedSpecialismAssessment.AssessmentSeriesId);
            actualSpecialismAssessment.IsOptedin.Should().Be(expectedSpecialismAssessment.IsOptedin);
            actualSpecialismAssessment.IsBulkUpload.Should().Be(expectedSpecialismAssessment.IsBulkUpload);
            actualSpecialismAssessment.StartDate.ToShortDateString().Should().Be(expectedSpecialismAssessment.StartDate.ToShortDateString());
            actualSpecialismAssessment.EndDate.Should().BeNull();
            actualSpecialismAssessment.CreatedBy.Should().Be(expectedSpecialismAssessment.CreatedBy);
        }

        private static void AssertPathwayAssessment(TqPathwayAssessment actualPathwayAssessment, TqPathwayAssessment expectedPathwayAssessment)
        {
            actualPathwayAssessment.TqRegistrationPathwayId.Should().Be(expectedPathwayAssessment.TqRegistrationPathwayId);
            actualPathwayAssessment.AssessmentSeriesId.Should().Be(expectedPathwayAssessment.AssessmentSeriesId);
            actualPathwayAssessment.IsOptedin.Should().Be(expectedPathwayAssessment.IsOptedin);
            actualPathwayAssessment.IsBulkUpload.Should().Be(actualPathwayAssessment.IsBulkUpload);
            actualPathwayAssessment.StartDate.ToShortDateString().Should().Be(expectedPathwayAssessment.StartDate.ToShortDateString());
            actualPathwayAssessment.EndDate.Should().BeNull();
            actualPathwayAssessment.CreatedBy.Should().Be(expectedPathwayAssessment.CreatedBy);
        }

        #region Dataseed methods
        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null, bool hasHistory = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);

            if (hasHistory)
            {
                // History - previously transferred
                var tqRegistrationPathwayHistory = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
                tqRegistrationPathwayHistory.Status = RegistrationPathwayStatus.Transferred;

                //// current active record.
                //tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
                //var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);
            }

            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            tqRegistrationPathway.Status = status;
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            DbContext.SaveChanges();
            return profile;
        }

        #endregion
    }
}
