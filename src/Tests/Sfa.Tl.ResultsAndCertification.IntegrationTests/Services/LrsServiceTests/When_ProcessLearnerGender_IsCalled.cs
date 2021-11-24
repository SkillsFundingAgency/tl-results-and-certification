using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.LrsServiceTests
{
    public class When_ProcessLearnerGender_IsCalled : LrsServiceBaseTest
    {
        private IList<TqRegistrationProfile> _profilesData;
        private LrsLearnerGenderResponse _result;
        private List<LrsLearnerRecordDetails> _learnerRecords;
        private List<(long uln, bool seedGender)> _testCriteriaData;

        public override void Given()
        {
            CreateMapper();
            SeedData();

            _profilesData = SeedRegistrationProfilesData(false);
            _learnerRecords = new List<LrsLearnerRecordDetails>();
            _testCriteriaData = new List<(long uln, bool seedGender)>
            {
                (1111111111, false),
                (1111111112, true),
                (1111111113, true),
                (1111111114, false),
                (1111111115, true)
            };

            foreach (var (uln, seedGender) in _testCriteriaData)
            {
                var profile = _profilesData.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                _learnerRecords.Add(BuildLearnerRecordDetails(profile, false, false, false, seedGender));
            }

            LrsServiceLogger = new Logger<LrsService>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);

            QualificationRepositoryLogger = new Logger<GenericRepository<Qualification>>(new NullLoggerFactory());
            QualificationRepository = new GenericRepository<Qualification>(QualificationRepositoryLogger, DbContext);

            LrsService = new LrsService(Mapper, LrsServiceLogger, RegistrationRepository, QualificationRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await LrsService.ProcessLearnerGenderAsync(_learnerRecords);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            // Assert result            
            _result.Should().NotBeNull();

            _result.IsSuccess.Should().BeTrue();
            _result.ModifiedCount.Should().Be(_learnerRecords.Count(x => x.Gender != null));
            _result.SavedCount.Should().Be(_learnerRecords.Count(x => x.Gender != null));

            var actualRegistrations = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => _testCriteriaData.Select(t => t.uln).Contains(x.UniqueLearnerNumber));

            actualRegistrations.Should().NotBeNull();

            foreach (var (uln, seedGender) in _testCriteriaData)
            {
                var expectedLearnerRecord = _learnerRecords.FirstOrDefault(l => l.Uln == uln);
                expectedLearnerRecord.Should().NotBeNull();

                var actualRegistrationProfile = actualRegistrations.FirstOrDefault(r => r.UniqueLearnerNumber == uln);

                actualRegistrationProfile.Should().NotBeNull();
                actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedLearnerRecord.Uln);
                actualRegistrationProfile.Firstname.Should().Be(expectedLearnerRecord.Firstname);
                actualRegistrationProfile.Lastname.Should().Be(expectedLearnerRecord.Lastname);
                actualRegistrationProfile.DateofBirth.Should().Be(expectedLearnerRecord.DateofBirth);
                actualRegistrationProfile.Gender.Should().Be(expectedLearnerRecord.Gender);
            }
        }
    }
}
