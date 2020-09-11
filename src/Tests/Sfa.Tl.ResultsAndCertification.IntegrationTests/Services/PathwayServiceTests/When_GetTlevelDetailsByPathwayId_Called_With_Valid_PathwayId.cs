using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayId_Called_With_Valid_PathwayId : PathwayServiceBaseTest
    {
        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public override void When()
        {
            _result = _service.GetTlevelDetailsByPathwayIdAsync(_tlAwardingOrganisation.UkPrn, _pathway.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var expectedResult = _result;
            expectedResult.Should().NotBeNull();
            expectedResult.RouteId.Should().Be(_route.Id);
            expectedResult.PathwayId.Should().Be(_pathway.Id);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.PathwayStatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.Specialisms.Should().NotBeNull();
            expectedResult.Specialisms.Count.Should().Be(_specialisms.Count);

            var expectedSpecialisms = _specialisms.Select(s => s.Name);
            _result.Specialisms.Should().Contain(expectedSpecialisms);
        }
    }
}
