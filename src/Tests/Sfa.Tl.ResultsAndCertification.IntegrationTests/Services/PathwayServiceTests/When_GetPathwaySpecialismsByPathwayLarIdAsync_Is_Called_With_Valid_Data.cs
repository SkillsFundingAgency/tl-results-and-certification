using System.Linq;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetPathwaySpecialismsByPathwayLarIdAsync_Is_Called_With_Valid_Data : PathwayServiceBaseTest
    {
        private PathwaySpecialisms _pathwaySpecialismsResult;
        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public override void When()
        {
            _pathwaySpecialismsResult = _service.GetPathwaySpecialismsByPathwayLarIdAsync(_tlAwardingOrganisation.UkPrn, _pathway.LarId).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var expectedResult = _pathwaySpecialismsResult;
            expectedResult.Should().NotBeNull();
            expectedResult.Id.Should().Be(_pathway.Id);
            expectedResult.PathwayCode.Should().Be(_pathway.LarId);
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.Specialisms.Should().NotBeNullOrEmpty();

            foreach(var specialism in expectedResult.Specialisms)
            {
                var actualSpecialism = _specialisms.FirstOrDefault(s => s.LarId == specialism.Code);

                actualSpecialism.Should().NotBeNull();
                actualSpecialism.LarId.Should().Be(specialism.Code);
                actualSpecialism.Name.Should().Be(specialism.Name);
            }
        }

        [Fact]
        public void Then_Pathway_Specialisms_Is_Not_Null()
        {
            _pathwaySpecialismsResult.Specialisms.Should().NotBeNull();
        }

        [Fact]
        public void Then_Pathway_Specialisms_Count_Is_As_Expected()
        {
            _pathwaySpecialismsResult.Specialisms.Count.Should().Be(_specialisms.Count);
        }
    }
}
