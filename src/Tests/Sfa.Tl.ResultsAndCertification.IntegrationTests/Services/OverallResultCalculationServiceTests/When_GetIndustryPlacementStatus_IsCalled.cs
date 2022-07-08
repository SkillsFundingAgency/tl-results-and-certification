using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetIndustryPlacementStatus_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private IndustryPlacementStatus _actualResult;

        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Seed Ip
            _ulnWithIndustryPlacements = new Dictionary<long, IndustryPlacementStatus>
            {
                { 1111111111, IndustryPlacementStatus.Completed },
                { 1111111112, IndustryPlacementStatus.CompletedWithSpecialConsideration },
                { 1111111113, IndustryPlacementStatus.NotCompleted }
            };
            SeedIndustyPlacementData(_ulnWithIndustryPlacements);
            DbContext.SaveChanges();

            // Dependencies
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    BatchSize = 10,
                    NoOfAcademicYearsToProcess = 4
                }
            };
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            OverallGradeLookupLogger = new Logger<GenericRepository<OverallGradeLookup>>(new NullLoggerFactory());
            OverallGradeLookupRepository = new GenericRepository<OverallGradeLookup>(OverallGradeLookupLogger, DbContext);
            AssessmentSeriesLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesLogger, DbContext);
            OverallResultLogger = new Logger<GenericRepository<OverallResult>>(new NullLoggerFactory());
            OverallResultRepository = new GenericRepository<OverallResult>(OverallResultLogger, DbContext);
            OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);
            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository, OverallResultRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(TqRegistrationPathway pathway)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetIndustryPlacementStatus(pathway);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, IndustryPlacementStatus expectedIpStatus)
        {
            var profile = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            await WhenAsync(profile.TqRegistrationPathways.FirstOrDefault());

            _actualResult.Should().Be(expectedIpStatus);
        }

        public void SeedIndustyPlacementData(Dictionary<long, IndustryPlacementStatus> ipUlns)
        {
            foreach (var ipUln in ipUlns)
            {
                var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == ipUln.Key).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipUln.Value);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, IndustryPlacementStatus.Completed },
                    new object[] { 1111111112, IndustryPlacementStatus.CompletedWithSpecialConsideration },
                    new object[] { 1111111113, IndustryPlacementStatus.NotCompleted },
                    new object[] { 1111111114, IndustryPlacementStatus.NotSpecified }
                };
            }
        }
    }
}