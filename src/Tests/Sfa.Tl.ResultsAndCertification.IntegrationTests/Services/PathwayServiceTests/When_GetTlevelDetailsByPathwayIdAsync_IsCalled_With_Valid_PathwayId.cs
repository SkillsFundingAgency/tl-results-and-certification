using System.Linq;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayIdAsync_IsCalled_With_Valid_PathwayId : PathwayServiceBaseTest
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
        }

        [Fact]
        public void Then_Pathway_Specialisms_Is_Not_Null()
        {
            _result.Specialisms.Should().NotBeNull();
        }

        [Fact]
        public void Then_Pathway_Specialisms_Count_Is_As_Expected()
        {
            _result.Specialisms.Count.Should().Be(_specialisms.Count);
        }

        [Fact]
        public void Then_Pathway_Specialisms_Data_As_Expected()
        {
            var expectedSpecialisms = _specialisms.Select(s => s.Name);
            _result.Specialisms.Should().Contain(expectedSpecialisms);
        }
    }
}
