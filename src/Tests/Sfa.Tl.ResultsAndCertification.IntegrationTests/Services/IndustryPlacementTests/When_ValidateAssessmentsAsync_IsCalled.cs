using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_ValidateAssessmentsAsync_IsCalled : IndustryPlacementServiceBaseTest
    {
        private IList<IndustryPlacementCsvRecordResponse> _csvData;
        private Task<IList<IndustryPlacementRecordResponse>> _stage3Result;
        protected IBlobStorageService BlobStorageService;
        private long _providerUkprn;

        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus>
        {
            { 1111111111, RegistrationPathwayStatus.Withdrawn },
            { 1111111112, RegistrationPathwayStatus.Active },
            { 1111111113, RegistrationPathwayStatus.Active },
            { 1111111114, RegistrationPathwayStatus.Active },
            { 1111111115, RegistrationPathwayStatus.Active },
            { 1111111116, RegistrationPathwayStatus.Active }
        };

        public override void Given()
        {
            CreateMapper();

            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);
            _ = SeedRegistrationsData(_ulns, TqProvider);

            // Create Service
            CreateMapper();

            IpLookupRepositoryLogger = new Logger<GenericRepository<IpLookup>>(new NullLoggerFactory());
            IpLookupRepository = new GenericRepository<IpLookup>(IpLookupRepositoryLogger, DbContext);

            IndustryPlacementLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            IndustryPlacementServiceLogger = new Logger<IndustryPlacementService>(new NullLoggerFactory());

            BlobStorageService = Substitute.For<IBlobStorageService>();

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IndustryPlacementRepository, RegistrationPathwayRepository, BlobStorageService,Mapper, IndustryPlacementServiceLogger);

            // setup input parameter
            SetupInputParameter();
        }

        public override Task When()
        {
            _stage3Result = IndustryPlacementService.ValidateIndustryPlacementsAsync(_providerUkprn, _csvData);
            return Task.CompletedTask;
        }

        [Fact]
        public void Expected_Results_Are_Returned()
        {
            var actualResult = _stage3Result.Result;
            actualResult.Should().NotBeNull();
            actualResult.Count().Should().Be(_csvData.Count());

            var expectedResult = new List<IndustryPlacementRecordResponse> 
            {
                new IndustryPlacementRecordResponse { TqRegistrationPathwayId = 1, IpStatus = 1 },
                new IndustryPlacementRecordResponse { ValidationErrors = new List<BulkProcessValidationError> 
                {
                    new BulkProcessValidationError { RowNum = "2", Uln = "9999999999", ErrorMessage = ValidationMessages.IpBulkUlnNotRegistered }
                } },
                new IndustryPlacementRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "3", Uln = "1111111112", ErrorMessage = ValidationMessages.IpBulkCorecodeInvalid }
                } },
            };

            actualResult.Should().BeEquivalentTo(expectedResult);
        }
        
        private void SetupInputParameter()
        {
            // Param 1
            _providerUkprn = 10000536;

            // Param 2;
            _csvData = new List<IndustryPlacementCsvRecordResponse>
            {
                new IndustryPlacementCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "10123456", IndustryPlacementStatus = "Placement completed" },
                new IndustryPlacementCsvRecordResponse { RowNum = 2, Uln = 9999999999, CoreCode = "10123457", IndustryPlacementStatus = "Placement completed" },
                new IndustryPlacementCsvRecordResponse { RowNum = 3, Uln = 1111111112, CoreCode = "10123457", IndustryPlacementStatus = "Placement completed" },
            };
        }
    }
}
