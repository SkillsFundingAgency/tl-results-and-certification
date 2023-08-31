using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism
{
    public class SpecialismNameConverter : SpecialismConverterBase, IValueConverter<IEnumerable<TqRegistrationSpecialism>, string>
    {
        private readonly DoubleQuotedStringConverter _doubleQuotedStringConverter = new();

        public string Convert(IEnumerable<TqRegistrationSpecialism> sourceMember, ResolutionContext context)
        {
            if (sourceMember.IsNullOrEmpty())
            {
                return string.Empty;
            }
            
            string specialismName = sourceMember.Count() switch
            {
                1 => GetSingleSpecialismProperty(sourceMember, rs => rs.TlSpecialism.Name),
                2 => GetDualSpecialismProperty(sourceMember, ds => ds.Name),
                _ => string.Empty
            };

            return _doubleQuotedStringConverter.Convert(specialismName, null);
        }
    }
}