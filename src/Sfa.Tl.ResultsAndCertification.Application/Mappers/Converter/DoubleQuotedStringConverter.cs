using AutoMapper;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class DoubleQuotedStringConverter : IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
        {
            return $"\"{sourceMember}\"";
        }
    }
}