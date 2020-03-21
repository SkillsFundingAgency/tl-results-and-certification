using System.Linq;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests.GetAllTlevelsByUkprnAsync
{
    public class When_GetAllTlevelsByUkprnAsync_IsCalled_With_Correct_Ukprn : AwardingOrganisaionServiceBaseTest
    {
        public override void Given()
        {
            SeedTlevelTestData();
            CreateMapper();

            _logger = Substitute.For<ILogger<IRepository<TqAwardingOrganisation>>>();
            _service = new AwardingOrganisationService(_resultsAndCertificationConfiguration, Repository, null, _mapper, _logger);
        }

        public override void When()
        {
            _result = _service.GetAllTlevelsByUkprnAsync(_tlAwardingOrganisation.UkPrn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var expectedResult = _result.FirstOrDefault();
            expectedResult.Should().NotBeNull();
            expectedResult.PathwayName.Should().Be(_pathway.Name);
            expectedResult.RouteName.Should().Be(_route.Name);
            expectedResult.StatusId.Should().Be(_tqAwardingOrganisation.ReviewStatus);
        }
    }
}
