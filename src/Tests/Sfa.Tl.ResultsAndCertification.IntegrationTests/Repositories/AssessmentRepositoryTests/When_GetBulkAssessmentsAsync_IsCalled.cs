using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
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

        public override void Given()
        {
            // Parameters
            _aoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationsData(_ulns, TqProvider);

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

            // Uln: 1111111111 - Registration that has no assessments
            var actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111111);
            actualresult.Should().NotBeNull();
            actualresult.TqPathwayAssessments.Should().BeEmpty();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Should().BeEmpty();

            // Uln: 1111111112 - Registration that has history of assessments
            //TODO: below scenario
            //actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111112);
            //actualresult.Should().NotBeNull();
            //actualresult.TqRegistrationSpecialisms.FirstOrDefault().Should().NotBeNull();
            //actualresult.TqPathwayAssessments.Should().BeEmpty();
            //actualresult.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Should().BeEmpty();

            // Uln: 1111111113 - Registration that is in withdrawn status and no specialism available
            actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111113);
            actualresult.Should().NotBeNull();
            actualresult.TqPathwayAssessments.Should().BeEmpty();
            actualresult.TqRegistrationSpecialisms.Should().BeEmpty();
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

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            tqRegistrationPathway.Status = status;
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            DbContext.SaveChanges();
            return profile;
        }

        #endregion
    }
}
