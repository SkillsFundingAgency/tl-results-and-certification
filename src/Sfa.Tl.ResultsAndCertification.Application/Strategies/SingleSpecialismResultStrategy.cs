﻿using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class SingleSpecialismResultStrategy : SpecialismResultStrategyBase
    {
        public override OverallSpecialismResultDetail GetResult(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null)
                throw new ArgumentNullException(nameof(specialisms), "The specialism collection cannot be null.");

            if (specialisms.Count != 1)
                throw new ArgumentException("The specialism collection must contain a single specialism.", nameof(specialisms));

            TqRegistrationSpecialism specialism = specialisms.Single();
            TqSpecialismResult result = GetHighestResult(specialism);

            return new OverallSpecialismResultDetail
            {
                SpecialismDetails = new List<OverallSpecialismDetail> { CreateOverallSpecialismDetail(specialism, result) },
                TlLookupId = result?.TlLookupId,
                OverallSpecialismResult = result?.TlLookup?.Value
            };
        }
    }
}