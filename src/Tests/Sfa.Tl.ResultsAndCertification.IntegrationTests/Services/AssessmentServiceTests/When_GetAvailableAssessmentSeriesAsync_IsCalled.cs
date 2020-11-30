using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_GetAvailableAssessmentSeriesAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AvailableAssessmentSeries _actualResult;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId, AssessmentEntryType assessmentEntryType)
        {
            if (_actualResult != null)
                return;

            _actualResult = await AssessmentService.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, assessmentEntryType);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(RequestParameter request, AvailableAssessmentSeries expectedResult)
        {
            await WhenAsync(request.AoUkprn, request.ProfileId, request.AssessmentEntryType);

            if (_actualResult == null)
            {
                expectedResult.Should().BeNull();
                return;
            }

            // Assert
            _actualResult.ProfileId.Should().Be(expectedResult.ProfileId);
            _actualResult.AssessmentSeriesId.Should().Be(expectedResult.AssessmentSeriesId);
            _actualResult.AssessmentSeriesName.Should().Be(expectedResult.AssessmentSeriesName);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // core assessment window opend
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, AssessmentEntryType = AssessmentEntryType.Core },
                      new AvailableAssessmentSeries { ProfileId = 1, AssessmentSeriesId = 1, AssessmentSeriesName = "Summer 2021" } },

                    // specialism assessment window not opened
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, AssessmentEntryType = AssessmentEntryType.Specialism },
                      null },

                    // invalid profil id
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 2, AssessmentEntryType = AssessmentEntryType.Core },
                      null }
                };
            }
        }
    }

    public class RequestParameter
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public AssessmentEntryType AssessmentEntryType { get; set; }
    }
}
