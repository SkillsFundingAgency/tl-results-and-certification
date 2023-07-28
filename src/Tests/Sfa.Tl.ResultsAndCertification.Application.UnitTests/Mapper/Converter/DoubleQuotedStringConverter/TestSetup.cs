using TheDoubleQuotedStringConverter = Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.DoubleQuotedStringConverter;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.Converter.DoubleQuotedStringConverter
{
    public abstract class TestSetup : ConverterBaseTest<TheDoubleQuotedStringConverter, string, string>
    {
        protected string DoubleQuotedEmptyString = $"\"{string.Empty}\"";
    }
}