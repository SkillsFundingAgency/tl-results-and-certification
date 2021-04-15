using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.LearnerRecordServiceTests
{
    public class When_GetPendingGenderLearners_IsCalled : LearnerRecordServiceBaseTest
    {
        private IList<TqRegistrationProfile> _profilesData;
        private IList<RegisteredLearnerDetails> _result;

        public override void Given()
        {
            CreateMapper();

            _profilesData = SeedRegistrationProfilesData(false);

            LearnerRecordServiceLogger = new Logger<LearnerRecordService>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);

            QualificationRepositoryLogger = new Logger<GenericRepository<Qualification>>(new NullLoggerFactory());
            QualificationRepository = new GenericRepository<Qualification>(QualificationRepositoryLogger, DbContext);

            LrsService = new LearnerRecordService(Mapper, LearnerRecordServiceLogger, RegistrationRepository, QualificationRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await LrsService.GetPendingGenderLearnersAsync();
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            // Expected result
            var expectedProfiles = _profilesData.Where(p => p.Gender == null).ToList();

            expectedProfiles.Should().NotBeNullOrEmpty();

            var acutalProfiles = _result;

            acutalProfiles.Should().NotBeNullOrEmpty();

            acutalProfiles.Count.Should().Be(expectedProfiles.Count);

            foreach (var actualProfile in acutalProfiles)
            {
                var expectedProfile = expectedProfiles.FirstOrDefault(p => p.UniqueLearnerNumber == actualProfile.Uln);

                expectedProfile.Should().NotBeNull();

                actualProfile.ProfileId.Should().Be(expectedProfile.Id);
                actualProfile.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
                actualProfile.Firstname.Should().Be(expectedProfile.Firstname);
                actualProfile.Lastname.Should().Be(expectedProfile.Lastname);
                actualProfile.DateofBirth.Should().Be(expectedProfile.DateofBirth);
                actualProfile.Gender.Should().Be(expectedProfile.Gender);
            }
        }
    }
}
