using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.HighestAttainedCoreSeries.HighestAttainedCoreSeriesConverter
{
    public class When_Pathway_Assessments_Empty : TestSetup
    {
        public override void Given()
        {
            Source = Enumerable.Empty<TqPathwayAssessment>();
        }

        [Fact]
        public void Then_Return_Empty()
        {
            Result.Should().BeEmpty();
        }
    }
}