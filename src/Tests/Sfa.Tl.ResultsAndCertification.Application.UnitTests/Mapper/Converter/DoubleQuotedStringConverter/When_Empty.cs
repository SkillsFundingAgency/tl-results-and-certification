using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.DoubleQuotedStringConverter
{
    public class When_Empty : TestSetup
    {
        public override void Given()
        {
            Source = string.Empty;
        }

        [Fact]
        public void Then_Return_DoubleQuotedEmptyString()
        {
            Result.Should().Be(DoubleQuotedEmptyString);
        }
    }
}