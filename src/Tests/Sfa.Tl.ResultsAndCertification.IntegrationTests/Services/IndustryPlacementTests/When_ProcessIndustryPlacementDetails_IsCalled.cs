using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
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
                        RegistrationPathwayId = 1,
                        IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,
                        IndustryPlacementDetails = null,
                        PerformedBy = "Test User"
                    }, true }
                };
            }
        }
    }
}
