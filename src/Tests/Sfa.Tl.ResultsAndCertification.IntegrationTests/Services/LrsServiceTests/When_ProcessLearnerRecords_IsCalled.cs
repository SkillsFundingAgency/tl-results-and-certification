using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class When_ProcessLearnerRecords_IsCalled : LrsServiceBaseTest
    {
        private IList<TqRegistrationProfile> _profilesData;
        private LrsLearnerVerificationAndLearningEventsResponse _result;
        private List<LrsLearnerRecordDetails> _learnerRecords;
        private List<(long uln, bool seedLearnignEvents, bool isEnglishAchieved, bool isMathsAchieved)> _testCriteriaData;

        public override void Given()
        {
            CreateMapper();
            SeedData();

            _profilesData = SeedRegistrationProfilesData(false);
            _learnerRecords = new List<LrsLearnerRecordDetails>();
            _testCriteriaData = new List<(long uln,bool seedLearnignEvents,bool isEnglishAchieved ,bool isMathsAchieved)>
            {
                (1111111111, false, true, true), // no learning events
                (1111111112, true, false, true), // learning events , english not achieved, math achieved
                (1111111113, true, true, false), // learning events , english achieved, math not achieved
                (1111111114, true, false, false), // learning events , english not achieved, math not achieved
                (1111111115, true, true, true) // learning events , english not achieved, math not achieved
            };

            foreach (var (uln, seedLearnignEvents, isEnglishAchieved, isMathsAchieved) in _testCriteriaData)
            {
                var profile = _profilesData.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                _learnerRecords.Add(BuildLearnerRecordDetails(profile, seedLearnignEvents, isEnglishAchieved, isMathsAchieved));
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
            _result = await LrsService.ProcessLearnerRecordsAsync(_learnerRecords);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            // Assert result            
            _result.Should().NotBeNull();

            _result.IsSuccess.Should().BeTrue();
            _result.ModifiedCount.Should().Be(_learnerRecords.Count);
            _result.SavedCount.Should().Be(_learnerRecords.Count);

            var actualRegistrations = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => _testCriteriaData.Select(t => t.uln).Contains(x.UniqueLearnerNumber))
                                                                                    .Include(x => x.QualificationAchieved)
                                                                                       .ThenInclude(x => x.Qualification)
                                                                                           .ThenInclude(x => x.QualificationType)
                                                                                    .Include(x => x.QualificationAchieved)
                                                                                       .ThenInclude(x => x.QualificationGrade);

            actualRegistrations.Should().NotBeNull();

            foreach (var (uln, seedLearnignEvents, isEnglishAchieved, isMathsAchieved) in _testCriteriaData)
            {
                var expectedLearnerRecord = _learnerRecords.FirstOrDefault(l => l.Uln == uln);
                expectedLearnerRecord.Should().NotBeNull();

                var expectedUln = uln;
                var expectedIsLearnerVerified = expectedLearnerRecord.IsLearnerVerified;
                SubjectStatus? expectedMathsStatus = seedLearnignEvents == false ? null : isMathsAchieved ? SubjectStatus.AchievedByLrs : SubjectStatus.NotAchievedByLrs;
                SubjectStatus? expectedEnglishStatus = seedLearnignEvents == false ? null : isEnglishAchieved ? SubjectStatus.AchievedByLrs : SubjectStatus.NotAchievedByLrs;

                var actualRegistrationProfile = actualRegistrations.FirstOrDefault(r => r.UniqueLearnerNumber == expectedUln);

                actualRegistrationProfile.Should().NotBeNull();

                actualRegistrationProfile.IsLearnerVerified.Should().Be(expectedIsLearnerVerified);
                actualRegistrationProfile.EnglishStatus.Should().Be(expectedEnglishStatus);
                actualRegistrationProfile.MathsStatus.Should().Be(expectedMathsStatus);

                // assert qualifications achieved
                actualRegistrationProfile.QualificationAchieved.Count().Should().Be(expectedLearnerRecord.LearningEventDetails.Count);

                if(seedLearnignEvents)
                {
                    foreach(var expectedLearningEvent in expectedLearnerRecord.LearningEventDetails)
                    {
                        var actualQualificationAchieved = actualRegistrationProfile.QualificationAchieved.FirstOrDefault(q => q.Qualification.Code == expectedLearningEvent.QualificationCode);

                        actualQualificationAchieved.Should().NotBeNull();
                        actualQualificationAchieved.Qualification.Code.Should().Be(expectedLearningEvent.QualificationCode);
                        actualQualificationAchieved.QualificationGrade.Grade.Should().Be(expectedLearningEvent.Grade);
                        actualQualificationAchieved.IsAchieved.Should().Be(expectedLearningEvent.IsAchieved);
                    }
                } 
                else
                {
                    actualRegistrationProfile.QualificationAchieved.Should().BeEmpty();
                }
            }
        }
    }
}