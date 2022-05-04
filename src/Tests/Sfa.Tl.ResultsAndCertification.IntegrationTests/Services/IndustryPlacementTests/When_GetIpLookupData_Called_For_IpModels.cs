using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_GetIpLookupData_Called_For_IpModels : IndustryPlacementServiceBaseTest
    {
        private IList<IpLookupData> _actualResult;

        public override void Given()
        {
            CreateMapper();
            SeedIpModelTlevelCombinationsData();

            PathwayId = 1;

            IpLookupRepositoryLogger = new Logger<GenericRepository<Domain.Models.IpLookup>>(new NullLoggerFactory());
            IpLookupRepository = new GenericRepository<Domain.Models.IpLookup>(IpLookupRepositoryLogger, DbContext);

            IpModelTlevelCombinationLogger = new Logger<GenericRepository<Domain.Models.IpModelTlevelCombination>>(new NullLoggerFactory());
            IpModelTlevelCombinationRepository = new GenericRepository<Domain.Models.IpModelTlevelCombination>(IpModelTlevelCombinationLogger, DbContext);

            IpTempFlexTlevelCombinationLogger = new Logger<GenericRepository<Domain.Models.IpTempFlexTlevelCombination>>(new NullLoggerFactory());
            IpTempFlexTlevelCombinationRepository = new GenericRepository<Domain.Models.IpTempFlexTlevelCombination>(IpTempFlexTlevelCombinationLogger, DbContext);

            IpTempFlexNavigationLogger = new Logger<GenericRepository<Domain.Models.IpTempFlexNavigation>>(new NullLoggerFactory());
            IpTempFlexNavigationRepository = new GenericRepository<Domain.Models.IpTempFlexNavigation>(IpTempFlexNavigationLogger, DbContext);

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IpModelTlevelCombinationRepository, IpTempFlexTlevelCombinationRepository, IpTempFlexNavigationRepository, Mapper);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _actualResult = await IndustryPlacementService.GetIpLookupDataAsync(IpLookupType.IndustryPlacementModel, PathwayId);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            var expectedResult = DbContext.IpModelTlevelCombination.Where(x => x.IsActive && x.IpLookup.TlLookup.Category == IpLookupType.IndustryPlacementModel.ToString()).OrderBy(x => x.IpLookup.SortOrder)
                .Select(x => new IpLookupData
                {
                    Id = x.IpLookup.Id,
                    Name = x.IpLookup.Name,
                    StartDate = x.IpLookup.StartDate,
                    EndDate = x.IpLookup.EndDate,
                    ShowOption = x.IpLookup.ShowOption != null ? Convert.ToBoolean(x.IpLookup.ShowOption) : null
                }).ToList();

            _actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
