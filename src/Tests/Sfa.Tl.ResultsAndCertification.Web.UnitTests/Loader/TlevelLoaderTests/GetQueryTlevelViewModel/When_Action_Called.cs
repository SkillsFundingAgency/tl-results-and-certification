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

            ActualResult.TqAwardingOrganisationId.Should().Be(ApiClientResponse.TqAwardingOrganisationId);
            ActualResult.RouteId.Should().Be(ApiClientResponse.RouteId);
            ActualResult.PathwayId.Should().Be(ApiClientResponse.PathwayId);
            ActualResult.PathwayStatusId.Should().Be(ApiClientResponse.PathwayStatusId);

            ActualResult.IsBackToConfirmed.Should().BeFalse();
            ActualResult.Query.Should().BeNull();

            ActualResult.TlevelTitle.Should().Be(ApiClientResponse.TlevelTitle);
            ActualResult.PathwayDisplayName.Should().Be($"{ApiClientResponse.PathwayName}<br/>({ApiClientResponse.PathwayCode})");
            ActualResult.Specialisms.Should().NotBeNull();
            ActualResult.Specialisms.Count().Should().Be(ApiClientResponse.Specialisms.Count());
            ActualResult.Specialisms.Should().BeEquivalentTo(ApiClientResponse.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})"));
        }
    }
}
