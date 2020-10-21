using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetPathwaySpecialismsByPathwayLarId_Called_With_InValid_Data : PathwayServiceBaseTest
    {
        private readonly long _ukprn = 00000;
        private PathwaySpecialisms _pathwaySpecialismsResult;
        public override void Given()
        {
            SeedTlevelTestData(EnumAwardingOrganisation.Pearson);
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public async override Task When()
        {
            _pathwaySpecialismsResult = await _service.GetPathwaySpecialismsByPathwayLarIdAsync(_ukprn, _pathway.LarId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _pathwaySpecialismsResult.Should().BeNull();
        }
    }
}
