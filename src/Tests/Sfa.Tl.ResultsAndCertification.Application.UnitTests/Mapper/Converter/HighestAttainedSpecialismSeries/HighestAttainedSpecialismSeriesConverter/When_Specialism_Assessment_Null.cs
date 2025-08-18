using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedSpecialismSeries.HighestAttainedSpecialismSeriesConverter
{
    internal class When_Specialism_Assessment_Null : TestSetup
    {
        public override void Given()
        {
            Source = null;
        }

        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}
