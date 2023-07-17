using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public abstract class SpecialismConverterBase
    {
        protected static string GetSingleSpecialismProperty(TqRegistrationPathway registrationPathway, Func<TqRegistrationSpecialism, string> getPropertyValue)
        {
            TqRegistrationSpecialism registrationSpecialism = registrationPathway.TqRegistrationSpecialisms.FirstOrDefault();
            return registrationSpecialism != null ? getPropertyValue(registrationSpecialism) : string.Empty;
        }

        protected static string GetDualSpecialismProperty(TqRegistrationPathway registrationPathway, Func<TlDualSpecialism, string> getPropertyValue)
        {
            IEnumerable<TlDualSpecialism> dualSpecialisms = registrationPathway.TqRegistrationSpecialisms
                                                                .Select(p => p.TlSpecialism)
                                                                .SelectMany(s => s.TlDualSpecialismToSpecialisms)
                                                                .Select(p => p.DualSpecialism);

            IGrouping<int, TlDualSpecialism> dualSpecialism = dualSpecialisms.GroupBy(p => p.Id).FirstOrDefault(p => p.Count() == 2);
            return dualSpecialism != null ? getPropertyValue(dualSpecialism.First()) : string.Empty;
        }
    }
}