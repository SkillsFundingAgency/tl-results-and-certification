using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
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
        private bool _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active }
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedSpecialConsiderationsLookupData();
            SeedIpModelTlevelCombinationsData(Pathway);
            SeedIpTempFlexTlevelCombinationsData(Pathway);

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            DbContext.SaveChanges();

            // Create Service
            CreateMapper();

            IpLookupRepositoryLogger = new Logger<GenericRepository<IpLookup>>(new NullLoggerFactory());
            IpLookupRepository = new GenericRepository<IpLookup>(IpLookupRepositoryLogger, DbContext);

            IpModelTlevelCombinationLogger = new Logger<GenericRepository<IpModelTlevelCombination>>(new NullLoggerFactory());
            IpModelTlevelCombinationRepository = new GenericRepository<IpModelTlevelCombination>(IpModelTlevelCombinationLogger, DbContext);

            IpTempFlexTlevelCombinationLogger = new Logger<GenericRepository<IpTempFlexTlevelCombination>>(new NullLoggerFactory());
            IpTempFlexTlevelCombinationRepository = new GenericRepository<IpTempFlexTlevelCombination>(IpTempFlexTlevelCombinationLogger, DbContext);

            IpTempFlexNavigationLogger = new Logger<GenericRepository<Domain.Models.IpTempFlexNavigation>>(new NullLoggerFactory());
            IpTempFlexNavigationRepository = new GenericRepository<Domain.Models.IpTempFlexNavigation>(IpTempFlexNavigationLogger, DbContext);

            IndustryPlacementLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            IndustryPlacementServiceLogger = new Logger<IndustryPlacementService>(new NullLoggerFactory());

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IpModelTlevelCombinationRepository, IpTempFlexTlevelCombinationRepository, IpTempFlexNavigationRepository, IndustryPlacementRepository, RegistrationPathwayRepository, Mapper, IndustryPlacementServiceLogger);
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

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted)
            {
                actualIndustryPlacement.Details.Should().BeNull();
            }
            else if (request.IndustryPlacementStatus == IndustryPlacementStatus.Completed || request.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
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
                    // Invalid Provider Ukprn - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = 0000000000,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, false },

                    // Invalid ProfileID - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 0,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, false },

                    // Invalid RegistrationPathwayId - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 0,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - NotSpecified - return false
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotSpecified,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - NotCompleted - return true
                    new object[] { new IndustryPlacementRequest
                    {
                        ProviderUkprn = (long)Provider.BarsleyCollege,
                        ProfileId = 1,
                        PathwayId = 1,
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true },

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
                            SpecialConsiderationReasons = new List<int?> { 1, 2 },
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - when MultipleEmployerModelsUsed = false then OtherIndustryPlacementModels should not have values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = false,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - when IndustryPlacementModelsUsed = false then MultipleEmployerModelsUsed & OtherIndustryPlacementModels should not have values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = false,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - when TemporaryFlexibilitiesUsed = false then BlendedTemporaryFlexibilityUsed & TemporaryFlexibilities should not have values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = false,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
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
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 12 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 16 }
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
                            SpecialConsiderationReasons = new List<int?> { 0, 2, 3, 4, 7 },
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 12 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 16 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - OtherIndustryPlacementModels values should be matched with IpLookup table - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 0, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - TemporaryFlexibilities values should be matched with IpLookup table return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 0 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - When TemporaryFlexibilitiesUsed and BlendedTemporaryFlexibilityUsed are false then  TemporaryFlexibilities should not have values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = false,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14 }
                        },
                        PerformedBy = "Test User"
                    }, false },
                    
                    // Ip Status - Completed - TemporaryFlexibilitiesUsed is null And BlendedTemporaryFlexibilityUsed is null then TemporaryFlexibilities list should not have any values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = null,
                            BlendedTemporaryFlexibilityUsed = null,
                            TemporaryFlexibilities = new List<int?> { 14 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - TemporaryFlexibilitiesUsed is true then TemporaryFlexibilities list should have values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = null,
                            TemporaryFlexibilities = new List<int?>()
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - TemporaryFlexibilitiesUsed is not used then TemporaryFlexibilities list should not have any values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = false,
                            BlendedTemporaryFlexibilityUsed = null,
                            TemporaryFlexibilities = new List<int?> { 14 }
                        },
                        PerformedBy = "Test User"
                    }, false },

                    // Ip Status - Completed - Temporary flexibility not exists and BlendedTemporaryFlexibilityUsed is false then TemporaryFlexibilities list should not have any values - return false
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = null,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14 }
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
                        IndustryPlacementDetails = new IndustryPlacementDetails
                        {
                            IndustryPlacementStatus = IndustryPlacementStatus.Completed.ToString(),
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 15 }
                        },
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
                            SpecialConsiderationReasons = new List<int?> { 1, 2, 3, 4, 7 },
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 12 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = true,
                            BlendedTemporaryFlexibilityUsed = false,
                            TemporaryFlexibilities = new List<int?> { 14, 16 }
                        },
                        PerformedBy = "Test User"
                    }, true },

                    // Ip Status - Completed - When TemporaryFlexibilitiesUsed = false and BlendedTemporaryFlexibilityUsed = true then TemporaryFlexibilities should have Blended Placement value - return true
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
                            HoursSpentOnPlacement = null,
                            SpecialConsiderationReasons = new List<int?>(),
                            IndustryPlacementModelsUsed = true,
                            MultipleEmployerModelsUsed = true,
                            OtherIndustryPlacementModels = new List<int?> { 10, 11 },
                            IndustryPlacementModels = new List<int?>(),
                            TemporaryFlexibilitiesUsed = null,
                            BlendedTemporaryFlexibilityUsed = true,
                            TemporaryFlexibilities = new List<int?>()
                        },
                        PerformedBy = "Test User"
                    }, true }
                };
            }
        }
    }
}
