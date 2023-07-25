using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.DoubleQuotedStringConverter
{
    public class When_Null : TestSetup
    {
        public override void Given()
        {
            Source = null;
        }

        [Fact]
        public void Then_Return_DoubleQuotedEmptyString()
        {
            Result.Should().Be(DoubleQuotedEmptyString);
        }
    }
}