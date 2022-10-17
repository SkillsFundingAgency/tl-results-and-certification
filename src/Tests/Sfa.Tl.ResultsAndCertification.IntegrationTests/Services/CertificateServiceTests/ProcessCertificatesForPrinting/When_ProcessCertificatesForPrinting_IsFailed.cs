using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Certificates;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests.ProcessCertificatesForPrinting
{
    public class When_ProcessCertificatesForPrinting_IsFailed : CertificateServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<OverallResult> _expectedOverallResults = new List<OverallResult>();
        private List<CertificateResponse> _actualResult;
        private ICertificateRepository _certificateRepository;
        private List<CertificateResponse> _expectedResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },    // Valid (Barnsley College)
                { 1111111112, RegistrationPathwayStatus.Active },    // Valid (Barnsley College)
                { 1111111113, RegistrationPathwayStatus.Active },    // Valid (Bishop Burton College)
                { 1111111114, RegistrationPathwayStatus.Active },    // Valid (Bishop Burton College)
                { 1111111115, RegistrationPathwayStatus.Active },    // Valid (Wallsal)
                { 1111111116, RegistrationPathwayStatus.Active }     // Valid (Walsall)
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            SeedTqProvider(Provider.BishopBurtonCollege);
            SetRegistrationProviders(_registrations, new List<long> { 1111111113, 1111111114 }, Provider.BishopBurtonCollege);

            SeedTqProvider(Provider.WalsallCollege);
            SetRegistrationProviders(_registrations, new List<long> { 1111111115, 1111111116 }, Provider.WalsallCollege);

            // Seed Overall results for all learners
            foreach (var registration in _registrations)
            {
                _expectedOverallResults.Add(new OverallResultCustomBuilder()
                    .WithTqRegistrationPathwayId(GetPathwayId(registration.UniqueLearnerNumber))
                    .WithPrintAvailableFrom(DateTime.Now.AddDays(-1))
                    .WithCalculationStatus(CalculationStatus.Completed)
                    .WithCertificateStatus(CertificateStatus.AwaitingProcessing)
                    .Save(DbContext));
            }


            // Create CertificateService
            CreateService();

            var failedMsg = "Failed to Save Batch";
            var certificateDataResponse = new CertificateDataResponse { IsSuccess = false, Message = failedMsg, BatchId = 0, TotalBatchRecordsCreated = 0, OverallResultsUpdatedCount = 0 };
            _expectedResult = new List<CertificateResponse>
            {
                new CertificateResponse {IsSuccess = false, Message = failedMsg, BatchId = 0, ProvidersCount = 2, CertificatesCreated = 0, OverallResultsUpdatedCount = 0 },
                new CertificateResponse {IsSuccess = false, Message = failedMsg, BatchId = 0, ProvidersCount = 1, CertificatesCreated = 0, OverallResultsUpdatedCount = 0 },
            };

            _certificateRepository.SaveCertificatesPrintingDataAsync(Arg.Any<Batch>(), Arg.Any<List<OverallResult>>()).Returns(certificateDataResponse);
        }

        public override async Task When()
        {
            _actualResult = await CertificateService.ProcessCertificatesForPrintingAsync();
        }

        [Fact]
        public void Then_ExpectedResults_Are_Returned()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(2);

            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }

        private int GetPathwayId(long uln)
        {
            return _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
        }

        protected override void CreateService()
        {
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                CertificatePrintingBatchSettings = new CertificatePrintingBatchSettings
                {
                    ProvidersBatchSize = 2
                }
            };

            var overallResultLogger = new Logger<GenericRepository<OverallResult>>(new NullLoggerFactory());
            var overallResultRepository = new GenericRepository<OverallResult>(overallResultLogger, DbContext);

            var certificateRepositoryLogger = new Logger<CertificateRepository>(new NullLoggerFactory());
            _certificateRepository = Substitute.For<ICertificateRepository>();


            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CertificateMapper).Assembly));
            var mapper = new Mapper(mapperConfig);

            // Create Service class to test. 
            CertificateService = new CertificateService(ResultsAndCertificationConfiguration, overallResultRepository, _certificateRepository, mapper);
        }

    }
}
