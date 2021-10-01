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
            var actualResult = _pathwaySpecialismsResult;
            actualResult.Should().NotBeNull();
            actualResult.Specialisms.Should().NotBeNull();
            actualResult.Id.Should().Be(_pathway.Id);
            actualResult.PathwayCode.Should().Be(_pathway.LarId);
            actualResult.PathwayName.Should().Be(_pathway.Name);
            
            // Should be two specialisms (one is couplet and other is single)
            actualResult.Specialisms.Count().Should().Be(2);
           
            // Couplet is with id 1 and 2
            var coupletSpecialism = actualResult.Specialisms.SingleOrDefault(x => x.SpecialismDetails.Count() == 2 && 
                    x.SpecialismDetails.Any(s => s.Id == 1) && x.SpecialismDetails.Any(s => s.Id == 1));
            coupletSpecialism.Should().NotBeNull();

            // single specialism with id 3
            var singleSpecialism = actualResult.Specialisms.SingleOrDefault(x => x.SpecialismDetails.Count() == 1 &&
                    x.SpecialismDetails.Any(s => s.Id == 3));
            singleSpecialism.Should().NotBeNull();

            foreach (var actualSpecialism in actualResult.Specialisms.SelectMany(x => x.SpecialismDetails))
            {
                var specialism = _specialisms.FirstOrDefault(s => s.LarId == actualSpecialism.Code);
                specialism.Should().NotBeNull();

                actualSpecialism.Id.Should().Be(specialism.Id);
                actualSpecialism.Name.Should().Be(specialism.Name);
                actualSpecialism.Code.Should().Be(specialism.LarId);
            }
        }
    }
}
