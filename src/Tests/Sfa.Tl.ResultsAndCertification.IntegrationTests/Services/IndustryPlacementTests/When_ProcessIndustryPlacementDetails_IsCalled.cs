using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_ProcessIndustryPlacementDetails_IsCalled : IndustryPlacementServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private List<IndustryPlacementData> _industryPlacementDatas;
        protected IBlobStorageService BlobStorageService;
        private bool _actualResult;


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
                new IndustryPlacementData { Uln = 1111111113, IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted, Details = null },
                new IndustryPlacementData { Uln = 1111111114, IndustryPlacementStatus = IndustryPlacementStatus.Completed, Details = new IndustryPlacementDetails { IndustryPlacementStatus = IndustryPlacementStatus.Completed.ToString() } },
                new IndustryPlacementData { Uln = 1111111115, IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = new IndustryPlacementDetails { HoursSpentOnPlacement = 100, SpecialConsiderationReasons = new List<int?> { 1, 3 } } },
                new IndustryPlacementData { Uln = 1111111114, IndustryPlacementStatus = IndustryPlacementStatus.Completed, Details = null }
            };

            SeedIndustyPlacementData(_industryPlacementDatas);
            DbContext.SaveChanges();

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

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IndustryPlacementRepository, RegistrationPathwayRepository, BlobStorageService, Mapper, IndustryPlacementServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(IndustryPlacementRequest request)
        {
            _actualResult = await IndustryPlacementService.ProcessIndustryPlacementDetailsAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(IndustryPlacementRequest request, bool expectedResult)
        {
            await WhenAsync(request);

            if (expectedResult == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            var expectedProfile = _profiles.FirstOrDefault(p => p.Id == request.ProfileId);

            expectedProfile.Should().NotBeNull();

            _actualResult.Should().BeTrue();

            var actualIndustryPlacement = DbContext.IndustryPlacement.FirstOrDefault(ip => ip.TqRegistrationPathwayId == request.RegistrationPathwayId);

            actualIndustryPlacement.Should().NotBeNull();

            actualIndustryPlacement.Status.Should().Be(request.IndustryPlacementStatus);

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.Completed || request.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted)
            {
                actualIndustryPlacement.Details.Should().BeNull();
            }
            else if (request.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                var actualDetails = JsonConvert.DeserializeObject<IndustryPlacementDetails>(actualIndustryPlacement.Details);
                actualDetails.Should().NotBeNull();

                actualDetails.Should().BeEquivalentTo(request.IndustryPlacementDetails);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    

                    // Ip Status - Completed - but populated with special considerations data - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.Completed.ToString(),
                            HoursSpentOnPlacement = 50,
                            SpecialConsiderationReasons = new List<int?> { 1, 2 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - CompletedWithSpecialConsideration - HoursSpentOnPlacement && SpecialConsiderationReasons should have values - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration.ToString(),
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>()
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - CompletedWithSpecialConsideration - SpecialConsiderationReasons values should be matched with IpLookup table - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration.ToString(),
                            HoursSpentOnPlacement = 50,
                            SpecialConsiderationReasons = new List<int?> { 0, 2, 3, 4, 7 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - return true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - CompletedWithSpecialConsideration - return true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration.ToString(),
                            HoursSpentOnPlacement = 50,
                            SpecialConsiderationReasons = new List<int?> { 1, 2, 3, 4, 7 }
                        },
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - NotCompleted - Changing it to Completed - return - true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 3,
                        PathwayId = 1,
                        RegistrationPathwayId = 3,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - Completed - Changing it to CompletedWithSpecialConsideration - return - true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 4,
                        PathwayId = 1,
                        RegistrationPathwayId = 4,
                        IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration.ToString(),
                            HoursSpentOnPlacement = 500,
                            SpecialConsiderationReasons = new List<int?>{ 1, 7 }
                        },
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - CompletedWithSpecialConsideration - Changing it to Completed - return - true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 5,
                        PathwayId = 1,
                        RegistrationPathwayId = 5,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - CompletedWithSpecialConsideration - Changing it to CompletedWithSpecialConsideration (updating Hours & Special Considerations) - return - true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 5,
                        PathwayId = 1,
                        RegistrationPathwayId = 5,
                        IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration.ToString(),
                            HoursSpentOnPlacement = 500,
                            SpecialConsiderationReasons = new List<int?>{ 1, 7 }
                        },
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - Completed - set it to Completed only - return - true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 6,
                        PathwayId = 1,
                        RegistrationPathwayId = 6,
                        IndustryPlacementStatus = IndustryPlacementStatus.Completed,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true }
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
    }

    public class IndustryPlacementData
    {
        public long Uln { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }
        public IndustryPlacementDetails Details { get; set; }
    }
}
