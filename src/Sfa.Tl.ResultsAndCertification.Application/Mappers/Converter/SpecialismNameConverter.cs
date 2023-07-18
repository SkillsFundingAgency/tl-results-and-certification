using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class SpecialismNameConverter : SpecialismConverterBase, IValueConverter<TqRegistrationPathway, string>
    {
        public string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            string specialismName = sourceMember.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismProperty(sourceMember, rs => rs.TlSpecialism.Name),
                2 => GetDualSpecialismProperty(sourceMember, ds => ds.Name),
                _ => string.Empty
            };

            return $"\"{specialismName}\"";
        }
    }
}