using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_UpdateLearnerSubjectAsync_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, SubjectStatus? MathsStatus, SubjectStatus? EnglishStatus)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private bool _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, SubjectStatus? MathsStatus, SubjectStatus? EnglishStatus)>
            {
                (1111111111, null, null), 
                (1111111112, SubjectStatus.AchievedByLrs, SubjectStatus.AchievedByLrs),
                (1111111113, null, null),
                (1111111114, null, null),
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            foreach (var (uln, mathsStatus, englishStatus) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerSubjectCriteria(profile, mathsStatus, englishStatus);
            }

            DbContext.SaveChanges();

            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            TrainingProviderRepositoryLogger = new Logger<TrainingProviderRepository>(new NullLoggerFactory());
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TrainingProviderRepositoryLogger);

            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, TrainingProviderRepository, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(UpdateLearnerSubjectRequest request)
        {
            _actualResult = await TrainingProviderService.UpdateLearnerSubjectAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(UpdateLearnerSubjectRequest request, bool expectedResult)
        {
            var expectedProfile = _profiles.FirstOrDefault(p => p.Id == request.ProfileId);
            expectedProfile.Should().NotBeNull();

            request.ProfileId = expectedProfile.Id;

            await WhenAsync(request);

            if (expectedResult == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            _actualResult.Should().BeTrue();

            var actualProfile = await DbContext.TqRegistrationProfile.FirstOrDefaultAsync(p => p.Id == expectedProfile.Id && p.UniqueLearnerNumber == expectedProfile.UniqueLearnerNumber
                                                                        && p.TqRegistrationPathways.Any(pa => pa.TqProvider.TlProvider.UkPrn == request.ProviderUkprn
                                                                        && (pa.Status == RegistrationPathwayStatus.Active || pa.Status == RegistrationPathwayStatus.Withdrawn)));

            actualProfile.Should().NotBeNull();

            // Assert Profile data
            actualProfile.Id.Should().Be(expectedProfile.Id);
            actualProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);

            // Assert EnglishAndMaths data
            if (request.SubjectType == SubjectType.Maths)
                actualProfile.MathsStatus.Should().Be(request.SubjectStatus);
            if (request.SubjectType == SubjectType.English)
                actualProfile.EnglishStatus.Should().Be(request.SubjectStatus);

            actualProfile.ModifiedBy.Should().Be(request.PerformedBy);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Invalid Provider Ukprn - return false
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = 0000000000, ProfileId = 1, SubjectStatus = SubjectStatus.Achieved, SubjectType = SubjectType.Maths, PerformedBy = "Test User" }, false },

                    // Invalid Subject status - return false
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = (long)Provider.BarnsleyCollege, ProfileId = 1, SubjectStatus = SubjectStatus.NotSpecified, SubjectType = SubjectType.Maths, PerformedBy = "Test User" }, false },

                    // Invalid Subject type - return false
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = (long)Provider.BarnsleyCollege, ProfileId = 1, SubjectStatus = SubjectStatus.Achieved, SubjectType = SubjectType.NotSpecified, PerformedBy = "Test User" }, false },

                    // Invalid when status already present - return false
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = (long)Provider.BarnsleyCollege, ProfileId = 2, SubjectStatus = SubjectStatus.Achieved, SubjectType = SubjectType.Maths, PerformedBy = "Test User" }, false },

                    // Valid Maths status - return true
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = (long)Provider.BarnsleyCollege, ProfileId = 1, SubjectStatus = SubjectStatus.Achieved, SubjectType = SubjectType.Maths, PerformedBy = "Test User" }, true },

                    // Valid English status - return true
                    new object[]
                    { new UpdateLearnerSubjectRequest { ProviderUkprn = (long)Provider.BarnsleyCollege, ProfileId = 1, SubjectStatus = SubjectStatus.NotAchieved, SubjectType = SubjectType.English, PerformedBy = "Test User" }, true },
                };
            }
        }
    }
}
