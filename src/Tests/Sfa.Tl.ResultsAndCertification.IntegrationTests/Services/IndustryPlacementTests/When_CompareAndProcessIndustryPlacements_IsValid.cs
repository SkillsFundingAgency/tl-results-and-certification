using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_CompareAndProcessIndustryPlacements_IsValid : IndustryPlacementServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private List<IndustryPlacementData> _industryPlacementDatas;
        protected ICommonRepository CommonRepository;
        private IndustryPlacementProcessResponse _actualResult;
        protected IBlobStorageService BlobStorageService;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedSpecialConsiderationsLookupData();

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            _industryPlacementDatas = new List<IndustryPlacementData>
            {
                new IndustryPlacementData { Uln = 1111111114, IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed, Details = null },
                new IndustryPlacementData { Uln = 1111111115, IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = new IndustryPlacementDetails { HoursSpentOnPlacement = 100, SpecialConsiderationReasons = new List<int?> { 1, 3 } } },
            };

            SeedIndustyPlacementData(_industryPlacementDatas);
            DbContext.SaveChanges();

            // Create Service
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(List<IndustryPlacement> inputData)
        {
            _actualResult = await IndustryPlacementService.CompareAndProcessIndustryPlacementsAsync(inputData);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(List<IndustryPlacement> inputData, IndustryPlacementProcessResponse expectedResult, IndustryPlacement expectedDbRecord)
        {
            await WhenAsync(inputData);

            _actualResult.Should().BeEquivalentTo(expectedResult);

            var actualDbRecord = DbContext.IndustryPlacement.FirstOrDefault(x => x.TqRegistrationPathwayId == inputData.FirstOrDefault().TqRegistrationPathwayId);
            actualDbRecord.TqRegistrationPathwayId.Should().Be(expectedDbRecord.TqRegistrationPathwayId);
            actualDbRecord.Status.Should().Be(expectedDbRecord.Status);
            actualDbRecord.Details.Should().Be(expectedDbRecord.Details);
            actualDbRecord.CreatedOn.ToShortDateString().Should().Be(expectedDbRecord.CreatedOn.ToShortDateString());
            actualDbRecord.CreatedBy.Should().Be(expectedDbRecord.CreatedBy);
            actualDbRecord.ModifiedBy.Should().Be(expectedDbRecord.ModifiedBy);

            if (expectedDbRecord.ModifiedOn != null)
                actualDbRecord.ModifiedOn.Value.ToShortDateString().Should().Be(expectedDbRecord.ModifiedOn.Value.ToShortDateString());
            else
                actualDbRecord.ModifiedOn.Should().BeNull();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // New Industry placement 
                    new object[]
                    {
                        new List<IndustryPlacement>
                        {
                            new IndustryPlacement { TqRegistrationPathwayId = 1, Status = Common.Enum.IndustryPlacementStatus.Completed, Details = null, CreatedOn = DateTime.Today, CreatedBy = "Test user" }
                        },
                        new IndustryPlacementProcessResponse { IsSuccess = true },
                        new IndustryPlacement { TqRegistrationPathwayId = 1, Status = Common.Enum.IndustryPlacementStatus.Completed, Details = null, CreatedOn = DateTime.Today, CreatedBy = "Test user", ModifiedBy = null, ModifiedOn = null }
                    },

                    // Amend Industry placement  (from Completed --> CompletedWithSpecialConsideratoin)
                    new object[]
                    {
                        new List<IndustryPlacement>
                        {
                            new IndustryPlacement { TqRegistrationPathwayId = 4, Status = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = "{IndustryPlacementStatus: CompletedWithSpecialConsideration,HoursSpentOnPlacement:100,SpecialConsiderationReasons:[33,44]}", CreatedOn = System.DateTime.UtcNow, CreatedBy = "TestUser" }
                        },
                        new IndustryPlacementProcessResponse { IsSuccess = true },
                        new IndustryPlacement { TqRegistrationPathwayId = 4, Status = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = "{IndustryPlacementStatus: CompletedWithSpecialConsideration,HoursSpentOnPlacement:100,SpecialConsiderationReasons:[33,44]}", CreatedOn = DateTime.Today, CreatedBy = "TestUser", ModifiedBy = "TestUser", ModifiedOn = DateTime.Today}
                    },

                    // No change
                    new object[]
                    {
                        new List<IndustryPlacement>
                        {
                            new IndustryPlacement { TqRegistrationPathwayId = 4, Status = Common.Enum.IndustryPlacementStatus.Completed, Details = null, CreatedOn = DateTime.Today, CreatedBy = "TestUser" }
                        },
                        new IndustryPlacementProcessResponse { IsSuccess = true },
                        new IndustryPlacement { TqRegistrationPathwayId = 4, Status = Common.Enum.IndustryPlacementStatus.Completed, Details = null, CreatedOn = DateTime.Today, CreatedBy = "TestUser", ModifiedBy = null, ModifiedOn = null }
                    },
                };
            }
        }

        private void SeedIndustyPlacementData(List<IndustryPlacementData> ipDatas)
        {
            foreach (var ipData in ipDatas)
            {
                var pathway = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == ipData.Uln).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipData.IndustryPlacementStatus, ipData.Details != null ? JsonConvert.SerializeObject(ipData.Details) : null);
            }
        }

        private void CreateService()
        {
            CreateMapper();

            IpLookupRepositoryLogger = new Logger<GenericRepository<IpLookup>>(new NullLoggerFactory());
            IpLookupRepository = new GenericRepository<IpLookup>(IpLookupRepositoryLogger, DbContext);

            IndustryPlacementLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            CommonRepository = new CommonRepository(DbContext);

            IndustryPlacementServiceLogger = new Logger<IndustryPlacementService>(new NullLoggerFactory());

            BlobStorageService = Substitute.For<IBlobStorageService>();

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IndustryPlacementRepository, 
                RegistrationPathwayRepository, CommonRepository, BlobStorageService, Mapper, IndustryPlacementServiceLogger);
        }
    }
}
