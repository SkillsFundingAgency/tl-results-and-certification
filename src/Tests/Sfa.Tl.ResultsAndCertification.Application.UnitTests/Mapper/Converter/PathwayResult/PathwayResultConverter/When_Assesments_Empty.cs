using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.PathwayResult.PathwayResultConverter
{
    public class When_Assesments_Empty : TestSetup
    {
        public override void Given()
        {
            Source = Enumerable.Empty<TqPathwayAssessment>();
        }

        [Fact]
        public void Then_Return_Null()
        {
            Result.Should().BeNull();
        }
    }
}