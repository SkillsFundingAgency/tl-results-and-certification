using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetPathwaySpecialismsByPathwayLarId_Called_With_Valid_Data : PathwayServiceBaseTest
    {
        private PathwaySpecialisms _pathwaySpecialismsResult;
        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public async override Task When()
        {
            _pathwaySpecialismsResult = await _service.GetPathwaySpecialismsByPathwayLarIdAsync(_tlAwardingOrganisation.UkPrn, _pathway.LarId);
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var expectedResult = _pathwaySpecialismsResult;
            expectedResult.Should().NotBeNull();
            expectedResult.Specialisms.Should().NotBeNull();
            expectedResult.Specialisms.Count().Should().Be(_specialisms.Count);
            expectedResult.Id.Should().Be(_pathway.Id);
            expectedResult.PathwayCode.Should().Be(_pathway.LarId);
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.Specialisms.Should().NotBeNullOrEmpty();

            foreach(var specialism in expectedResult.Specialisms)
            {
                var actualSpecialism = _specialisms.FirstOrDefault(s => s.LarId == specialism.Code);
                actualSpecialism.Should().NotBeNull();
                actualSpecialism.LarId.Should().Be(specialism.Code);
            }
        }
    }
}
