﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public class When_GetAvailableAssessmentSeriesAsync_IsCalled : AssessmentRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<AssessmentSeries> _actualResult;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId, int startInYear)
        {
            if (_actualResult != null)
                return;

            _actualResult = await AssessmentRepository.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, startInYear);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(RequestParameter request, AssessmentSeries expectedResult)
        {
            await WhenAsync(request.AoUkprn, request.ProfileId, request.StartInYear);

            if (_actualResult == null || !_actualResult.Any())
            {
                expectedResult.Should().BeNull();
                return;
            }

            var actualAssessmentSeries = _actualResult.FirstOrDefault(a => a.ComponentType == request.ComponentType);

            if (expectedResult == null)
            {
                actualAssessmentSeries.Should().BeNull();
            }
            else
            {
                // Assert
                actualAssessmentSeries.Should().NotBeNull();
                actualAssessmentSeries.Id.Should().Be(expectedResult.Id);
                actualAssessmentSeries.Name.Should().Be(expectedResult.Name);
                actualAssessmentSeries.Year.Should().Be(expectedResult.Year);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, StartInYear = 0, ComponentType = ComponentType.Core },
                      new AssessmentSeries { Id = 1, ComponentType = ComponentType.Core, Name = "Summer 2021", Description = "Summer 2021", Year = 2021 } },

                    // Start in +1 yr - assessment window not opened.
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, StartInYear = 1, ComponentType = ComponentType.Core },
                       new AssessmentSeries { Id = 1, ComponentType = ComponentType.Core, Name = "Summer 2021", Description = "Summer 2021", Year = 2021 } },

                    // Invlaid AoUkprn
                    new object[]
                    { new RequestParameter { AoUkprn = 99999999, ProfileId = 1, StartInYear = 0, ComponentType = ComponentType.Core  },
                      null },

                    // Invalid ProfileId
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 99, StartInYear = 0, ComponentType = ComponentType.Core },
                      null },
                };
            }
        }
    }

    public class RequestParameter
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public int StartInYear { get; set; }
        public ComponentType ComponentType { get; set; }
    }
}
