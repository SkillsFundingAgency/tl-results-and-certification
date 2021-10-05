using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsToReviewByUkprn
{
    public class When_More_To_Review : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.TlevelsToReview.Should().NotBeNull();
            ActualResult.TlevelsToReview.Count().Should().Be(2);
            ActualResult.IsOnlyOneTlevelReviewPending.Should().BeFalse();
        }
    }
}
