using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayId_Called_For_Queried_Tlevel : PathwayServiceBaseTest
    {
        public override void Given()
        {
            SeedTlevelTestData();
            _tqAwardingOrganisation.ModifiedBy = "Test User";
            _tqAwardingOrganisation.ModifiedOn = DateTime.UtcNow;
            DbContext.SaveChanges();
            
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TlPathway>>>();
            _service = new PathwayService(Repository, _mapper);
        }

        public async override Task When()
        {
            _result = await _service.GetTlevelDetailsByPathwayIdAsync(_tlAwardingOrganisation.UkPrn, _pathway.Id);
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
            expectedResult.PathwayCode.Should().Be(_pathway.LarId);
            expectedResult.TlevelTitle.Should().Be(_pathway.TlevelTitle);
            expectedResult.PathwayStatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.Specialisms.Should().NotBeNull();
            expectedResult.Specialisms.Count.Should().Be(_specialisms.Count);
            expectedResult.VerifiedBy.Should().Be(_tqAwardingOrganisation.ModifiedBy);
            expectedResult.VerifiedOn.Should().Be(_tqAwardingOrganisation.ModifiedOn);

            var expectedSpecialisms = _specialisms.Select(s => new SpecialismDetails { Id = s.Id, Name = s.Name, Code = s.LarId }).ToList();
            expectedResult.Specialisms.Should().BeEquivalentTo(expectedSpecialisms);
        }
    }
}
