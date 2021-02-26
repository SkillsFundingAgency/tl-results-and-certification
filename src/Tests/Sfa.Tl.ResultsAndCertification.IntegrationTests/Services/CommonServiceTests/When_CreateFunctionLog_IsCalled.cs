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
    public class When_CreateFunctionLog_IsCalled : CommonServiceBaseTest
    {
        private FunctionLogDetails _functionLog;
        private bool _result;

        public override void Given()
        {
            CreateMapper();
            
            CommonServiceLogger = new Logger<CommonService>(new NullLoggerFactory());
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            FunctionLogRepositoryLogger = new Logger<GenericRepository<FunctionLog>>(new NullLoggerFactory());
            FunctionLogRepository = new GenericRepository<FunctionLog>(FunctionLogRepositoryLogger, DbContext);
            CommonService = new CommonService(CommonServiceLogger, CommonMapper, TlLookupRepository, FunctionLogRepository);

            _functionLog = new FunctionLogDetails 
            {
                Name = "Test",
                StartDate = DateTime.UtcNow,
                Status = FunctionStatus.Processing,
                PerformedBy = "Test User"
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(bool seedValidData)
        {
            _result = await CommonService.CreateFunctionLog(seedValidData ? _functionLog : null);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Expected_Results_Returned(bool seedValidData, bool expectedResponse)
        {
            await WhenAsync(seedValidData);

            _result.Should().Be(expectedResponse);

            if (!expectedResponse) return;

            var actualFunctionLog = DbContext.FunctionLog.FirstOrDefault(x => x.Name == _functionLog.Name);

            actualFunctionLog.Should().NotBeNull();

            actualFunctionLog.Name.Should().Be(_functionLog.Name);
            actualFunctionLog.StartDate.Should().Be(_functionLog.StartDate);
            actualFunctionLog.EndDate.Should().Be(_functionLog.EndDate);
            actualFunctionLog.Status.Should().Be(_functionLog.Status);
            actualFunctionLog.CreatedBy.Should().Be(_functionLog.PerformedBy);
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
