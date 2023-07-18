using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class SpecialismCodeConverter : SpecialismConverterBase, IValueConverter<TqRegistrationPathway, string>
    {
        public string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            string specialismCode = sourceMember.TqRegistrationSpecialisms.Count switch
            {
                1 => GetSingleSpecialismProperty(sourceMember, rs => rs.TlSpecialism.LarId),
                2 => GetDualSpecialismProperty(sourceMember, ds => ds.LarId),
                _ => string.Empty
            };

            return specialismCode;
        }
    }
}