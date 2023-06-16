using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class SpecialismResultStrategyFactory : ISpecialismResultStrategyFactory
    {
        public ISpecialismResultStrategy GetSpecialismResultStrategy(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null)
                throw new ArgumentNullException(nameof(specialisms), "The specialism collection cannot be null.");

            return specialisms.Count switch
            {
                1 => new SingleSpecialismResultStrategy(),
                _ => throw new ArgumentNullException(nameof(specialisms), "The specialism collection must cointain one or two specialisms")
            };
        }
    }
}