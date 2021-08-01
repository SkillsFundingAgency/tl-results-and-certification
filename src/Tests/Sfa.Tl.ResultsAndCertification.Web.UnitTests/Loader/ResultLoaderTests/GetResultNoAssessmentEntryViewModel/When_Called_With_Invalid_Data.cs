using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultNoAssessmentEntryViewModel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ResultDetailsViewModel = null;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
