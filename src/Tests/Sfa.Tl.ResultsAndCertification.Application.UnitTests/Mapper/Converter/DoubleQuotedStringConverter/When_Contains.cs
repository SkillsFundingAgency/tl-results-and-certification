using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.DoubleQuotedStringConverter
{
    public class When_Contains : TestSetup
    {
        private const char _doubleQuoteChar = '"';
        private const string _doubleQuote = "\"";

        public override void Given()
        {
            Source = "The string to be tested.";
        }

        [Fact]
        public void Then_Return_Expected()
        {
            Result.Should().StartWith(_doubleQuote).And.EndWith(_doubleQuote);
            Result.Trim(_doubleQuoteChar).Should().Be(Source);
        }
    }
}