using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public class When_GetTlevelDetailsByPathwayIdAsync_IsCalled_With_Valid_Id : PathwayServiceBaseTest
    {
        //private readonly long _ukprn = 10011881;
        //private readonly int _pathwayId = 1;
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
    }
}
