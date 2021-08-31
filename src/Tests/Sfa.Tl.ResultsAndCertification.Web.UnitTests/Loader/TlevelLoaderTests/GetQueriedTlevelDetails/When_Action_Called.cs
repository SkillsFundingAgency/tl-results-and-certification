using Xunit;
using NSubstitute;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueriedTlevelDetails
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetTlevelDetailsByPathwayIdAsync(Ukprn, PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedSpecialisms = new List<string> { "Civil Engineering<br/>(97865897)", "Assisting teaching<br/>(7654321)" };

            ActualResult.TlevelTitle.Should().Be(ApiClientResponse.TlevelTitle);
            ActualResult.PathwayDisplayName.Should().Be($"{ApiClientResponse.PathwayName}<br/>({ApiClientResponse.PathwayCode})");
            ActualResult.Specialisms.Should().NotBeNull();
            ActualResult.Specialisms.Count().Should().Be(ApiClientResponse.Specialisms.Count());
            ActualResult.Specialisms.Should().BeEquivalentTo(ApiClientResponse.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})"));
            ActualResult.QueriedBy.Should().Be(ApiClientResponse.VerifiedBy);
            ActualResult.QueriedOn.Should().Be(ApiClientResponse.VerifiedOn.Value.ToDobFormat());
            ActualResult.IsValid.Should().BeTrue();
        }
    }
}
