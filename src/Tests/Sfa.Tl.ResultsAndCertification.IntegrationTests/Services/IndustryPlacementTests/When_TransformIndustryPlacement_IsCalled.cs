using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public class When_TransformIndustryPlacement_IsCalled : IndustryPlacementServiceBaseTest
    {
        private List<IndustryPlacementRecordResponse> _industryPlacementData;
        protected IBlobStorageService BlobStorageService;
        private string _performedBy;

        private IList<IndustryPlacement> _actualResult;
        private IList<IndustryPlacement> _expectedResult;

        public override void Given()
        {

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

            _performedBy = "Test user";
            _industryPlacementData = new List<IndustryPlacementRecordResponse>
            {
                new IndustryPlacementRecordResponse 
                {
                    TqRegistrationPathwayId = 1,
                    IpStatus = 1,
                    IpHours = null,
                    SpecialConsiderationReasons = null
                },
                new IndustryPlacementRecordResponse
                {
                    TqRegistrationPathwayId = 2,
                    IpStatus = 2,
                    IpHours = 100,
                    SpecialConsiderationReasons = new List<int?> { 33, 44 },
                },
                new IndustryPlacementRecordResponse
                {
                    TqRegistrationPathwayId = 3,
                    IpStatus = 3,
                    IpHours = null,
                    SpecialConsiderationReasons = null
                },
                new IndustryPlacementRecordResponse
                {
                    TqRegistrationPathwayId = 4,
                    IpStatus = 4,
                    IpHours = null,
                    SpecialConsiderationReasons = null
                }
            };

            _expectedResult = new List<IndustryPlacement> 
            {
                new IndustryPlacement 
                {
                    TqRegistrationPathwayId = 1,
                    Status = Common.Enum.IndustryPlacementStatus.Completed,
                    Details = null,
                    CreatedBy = _performedBy
                },
                new IndustryPlacement
                {
                    TqRegistrationPathwayId = 2,
                    Status = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration,
                    Details = "",
                    CreatedBy = _performedBy
                },
                new IndustryPlacement
                {
                    TqRegistrationPathwayId = 3,
                    Status = Common.Enum.IndustryPlacementStatus.NotCompleted,
                    Details = null,
                    CreatedBy = _performedBy
                },
                new IndustryPlacement
                {
                    TqRegistrationPathwayId = 4,
                    Status = Common.Enum.IndustryPlacementStatus.WillNotComplete,
                    Details = null,
                    CreatedBy = _performedBy
                },
            };
        }

        public override Task When()
        {
            _actualResult = IndustryPlacementService.TransformIndustryPlacementsModel(_industryPlacementData, _performedBy);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(_expectedResult.Count);

            var idx = 0;
            foreach (var actualResult in _actualResult)
            {
                actualResult.TqRegistrationPathwayId.Should().Be(_expectedResult[idx].TqRegistrationPathwayId);
                actualResult.Status.Should().Be(_expectedResult[idx].Status);
                if (actualResult.Status == Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration)
                    actualResult.Details.Should().Be("{\"IndustryPlacementStatus\":\"CompletedWithSpecialConsideration\",\"HoursSpentOnPlacement\":100,\"SpecialConsiderationReasons\":[33,44]}");
                else
                    actualResult.Details.Should().Be(_expectedResult[idx].Details);
                
                actualResult.CreatedBy.Should().Be(_expectedResult[idx].CreatedBy);
                actualResult.CreatedOn.Should().BeWithin(5.Seconds()).Before(System.DateTime.UtcNow);
                idx++;
            }
        }
    }
}

