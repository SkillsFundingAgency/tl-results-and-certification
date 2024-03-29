﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_GetIpLookupData_Called_For_SpecialConsideration : IndustryPlacementServiceBaseTest
    {
        private IList<IpLookupData> _actualResult;
        protected ICommonRepository CommonRepository;
        protected IBlobStorageService BlobStorageService;


        public override void Given()
        {
            CreateMapper();
            SeedSpecialConsiderationsLookupData();

            IpLookupRepositoryLogger = new Logger<GenericRepository<IpLookup>>(new NullLoggerFactory());
            IpLookupRepository = new GenericRepository<IpLookup>(IpLookupRepositoryLogger, DbContext);

            IndustryPlacementLogger = new Logger<GenericRepository<Domain.Models.IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<Domain.Models.IndustryPlacement>(IndustryPlacementLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<Domain.Models.TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<Domain.Models.TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            CommonRepository = new CommonRepository(DbContext);

            IndustryPlacementServiceLogger = new Logger<IndustryPlacementService>(new NullLoggerFactory());

            BlobStorageService = Substitute.For<IBlobStorageService>();

            IndustryPlacementService = new IndustryPlacementService(IpLookupRepository, IndustryPlacementRepository, RegistrationPathwayRepository,
                CommonRepository, BlobStorageService, Mapper, IndustryPlacementServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _actualResult = await IndustryPlacementService.GetIpLookupDataAsync(IpLookupType.SpecialConsideration, null);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            var expectedResult = DbContext.IpLookup.Where(x => x.TlLookup.Category == IpLookupType.SpecialConsideration.ToString()).OrderBy(x => x.SortOrder)
                .Select(x => new IpLookupData
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    ShowOption = x.ShowOption != null ? Convert.ToBoolean(x.ShowOption) : null
                }).ToList();

            _actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
