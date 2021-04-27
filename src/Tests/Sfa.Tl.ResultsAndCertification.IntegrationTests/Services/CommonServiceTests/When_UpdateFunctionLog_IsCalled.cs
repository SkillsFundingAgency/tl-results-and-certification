using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CommonServiceTests
{
    public class When_UpdateFunctionLog_IsCalled : CommonServiceBaseTest
    {
        private FunctionLog _functionLog;
        private FunctionLogDetails _functionLogDetails;
        private bool _result;

        public override void Given()
        {
            CreateMapper();

            _functionLog = SeedFunctionLog();

            CommonServiceLogger = new Logger<CommonService>(new NullLoggerFactory());
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            FunctionLogRepositoryLogger = new Logger<GenericRepository<FunctionLog>>(new NullLoggerFactory());
            FunctionLogRepository = new GenericRepository<FunctionLog>(FunctionLogRepositoryLogger, DbContext);
            CommonRepository = new CommonRepository(DbContext);
            CommonService = new CommonService(CommonServiceLogger, CommonMapper, TlLookupRepository, FunctionLogRepository, CommonRepository);

            _functionLogDetails = new FunctionLogDetails
            {
                Id = _functionLog.Id,
                Name = _functionLog.Name,
                StartDate = _functionLog.StartDate,
                EndDate = DateTime.UtcNow,
                Status = FunctionStatus.Processed,
                PerformedBy = "Test User"
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(bool updateValidData)
        {
            if (!updateValidData) _functionLogDetails.Id = 0;
            _result = await CommonService.UpdateFunctionLog(_functionLogDetails);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Expected_Results_Returned(bool updateValidData, bool expectedResponse)
        {
            await WhenAsync(updateValidData);

            _result.Should().Be(expectedResponse);

            if (!expectedResponse) return;

            var actualFunctionLog = DbContext.FunctionLog.FirstOrDefault(x => x.Name == _functionLog.Name);

            actualFunctionLog.Should().NotBeNull();

            actualFunctionLog.Name.Should().Be(_functionLogDetails.Name);
            actualFunctionLog.StartDate.Should().Be(_functionLogDetails.StartDate);
            actualFunctionLog.EndDate.Should().Be(_functionLogDetails.EndDate);
            actualFunctionLog.Status.Should().Be(_functionLogDetails.Status);
            actualFunctionLog.ModifiedBy.Should().Be(_functionLogDetails.PerformedBy);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { true, true },
                    new object[] { false, false }
                };
            }
        }
    }
}
