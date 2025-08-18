using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedCoreSeries.HighestAttainedCoreSeriesConverter
{
    public class When_Pathway_Assessment_Null : TestSetup
    {
        public override void Given()
        {
            Source = null;
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}
