using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueryTlevelViewModel
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedSpecialisms = new List<string> { "Civil Engineering<br/>(97865897)", "Assisting teaching<br/>(7654321)" };

            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ApiClientResponse.PathwayId);
            ActualResult.PathwayName.Should().Be(ApiClientResponse.PathwayName);
            ActualResult.PathwayStatusId.Should().Be(ApiClientResponse.PathwayStatusId);
            ActualResult.TqAwardingOrganisationId.Should().Be(ApiClientResponse.TqAwardingOrganisationId);
            ActualResult.Specialisms.Should().NotBeNull();
            ActualResult.Specialisms.Count().Should().Be(2);
            ActualResult.Specialisms.Should().BeEquivalentTo(expectedSpecialisms);
            ActualResult.Query.Should().BeNull();
        }
    }
}
