using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism
{
    public class SpecialismCodeConverter : SpecialismConverterBase, IValueConverter<TqRegistrationPathway, string>
    {
        public string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            if (sourceMember.TqRegistrationSpecialisms.IsNullOrEmpty())
            {
                return string.Empty;
            }

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