using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.Specialism
{
    public abstract class SpecialismConverterBase
    {
        protected static string GetSingleSpecialismProperty(IEnumerable<TqRegistrationSpecialism> specialisms, Func<TqRegistrationSpecialism, string> getPropertyValue)
        {
            TqRegistrationSpecialism registrationSpecialism = specialisms.FirstOrDefault();
            return registrationSpecialism != null ? getPropertyValue(registrationSpecialism) : string.Empty;
        }

        protected static string GetDualSpecialismNameProperty(IEnumerable<TqRegistrationSpecialism> specialisms, Func<TlDualSpecialism, string> getPropertyValue)
        {
            IEnumerable<TlDualSpecialism> dualSpecialisms = specialisms
                                                                .Select(p => p.TlSpecialism)
                                                                .SelectMany(s => s.TlDualSpecialismToSpecialisms)
                                                                .Select(p => p.DualSpecialism);

            IGrouping<int, TlDualSpecialism> dualSpecialism = dualSpecialisms.GroupBy(p => p.Id).FirstOrDefault(p => p.Count() == 2);

            if (!dualSpecialisms.Any())
            {
                TqRegistrationSpecialism latestRegistrationSpecialism = specialisms.FirstOrDefault(f => f.EndDate is null);
                return latestRegistrationSpecialism != null ? latestRegistrationSpecialism.TlSpecialism.Name : string.Empty;
            }

            return dualSpecialism != null ? getPropertyValue(dualSpecialism.First()) : string.Empty;
        }

        protected static string GetDualSpecialismLarIdProperty(IEnumerable<TqRegistrationSpecialism> specialisms, Func<TlDualSpecialism, string> getPropertyValue)
        {
            IEnumerable<TlDualSpecialism> dualSpecialisms = specialisms
                                                                .Select(p => p.TlSpecialism)
                                                                .SelectMany(s => s.TlDualSpecialismToSpecialisms)
                                                                .Select(p => p.DualSpecialism);

            IGrouping<int, TlDualSpecialism> dualSpecialism = dualSpecialisms.GroupBy(p => p.Id).FirstOrDefault(p => p.Count() == 2);

            if (!dualSpecialisms.Any())
            {
                TqRegistrationSpecialism latestRegistrationSpecialism = specialisms.FirstOrDefault(f => f.EndDate is null);
                return latestRegistrationSpecialism != null ? latestRegistrationSpecialism.TlSpecialism.LarId : string.Empty;
            }

            return dualSpecialism != null ? getPropertyValue(dualSpecialism.First()) : string.Empty;
        }


    }
}