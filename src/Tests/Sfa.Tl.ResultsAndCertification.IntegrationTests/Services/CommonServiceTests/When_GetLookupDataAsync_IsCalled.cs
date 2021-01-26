using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CommonServiceTests
{
    public class When_GetLookupDataAsync_IsCalled : CommonServiceBaseTest
    {
        private IEnumerable<LookupData> _lookupResponse;

        public override void Given()
        {
            CreateMapper();
            SeedLookupData();

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            CommonService = new CommonService(TlLookupRepository, CommonMapper);
        }

        public override Task When()
        {
            _lookupResponse = CommonService.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
            return Task.CompletedTask;
        }

        [Fact]
        public void Expected_LookupData_Returned() 
        {
            _lookupResponse.Should().NotBeEmpty();
            _lookupResponse.Count().Should().Be(TlLookup.Count);
            var actualResponse = _lookupResponse.ToList();

            for (var i=0; i < _lookupResponse.Count(); i++)
            {
                actualResponse[i].Id.Should().Be(TlLookup[i].Id);
                actualResponse[i].Code.Should().Be(TlLookup[i].Code);
                actualResponse[i].Value.Should().Be(TlLookup[i].Value);
            }
        }
    }
}
