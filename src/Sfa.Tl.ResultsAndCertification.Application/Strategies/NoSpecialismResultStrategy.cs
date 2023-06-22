using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class NoSpecialismResultStrategy : ISpecialismResultStrategy
    {
        public OverallSpecialismResultDetail GetResult(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms != null && specialisms.Count > 0)
                throw new ArgumentException("The specialism collection must be null or empty.", nameof(specialisms));

            return new OverallSpecialismResultDetail
            {
                SpecialismDetails = new List<OverallSpecialismDetail>(),
                TlLookupId = null,
                OverallSpecialismResult = null
            };
        }
    }
}