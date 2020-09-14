using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayId_Called_With_InValid_PathwayId : PathwayServiceBaseTest
    {
        private readonly long _ukprn = 00000;
        private readonly int _pathwayId = 0;

        public override void Given()
        {
            SeedTlevelTestData(EnumAwardingOrganisation.Pearson);
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public async override Task When()
        {
            _result = await _service.GetTlevelDetailsByPathwayIdAsync(_ukprn, _pathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
        }
    }
}
