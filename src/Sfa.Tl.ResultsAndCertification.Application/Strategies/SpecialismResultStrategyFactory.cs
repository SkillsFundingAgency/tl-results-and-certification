using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class SpecialismResultStrategyFactory : ISpecialismResultStrategyFactory
    {
        private readonly IRepository<DualSpecialismOverallGradeLookup> _dualSpecialismOverallGradeLookupRepository;
        private readonly Dictionary<int, ISpecialismResultStrategy> _specialismStrategyDict = new();

        public SpecialismResultStrategyFactory(IRepository<DualSpecialismOverallGradeLookup> dualSpecialismOverallGradeLookupRepository)
        {
            _dualSpecialismOverallGradeLookupRepository = dualSpecialismOverallGradeLookupRepository;
        }

        public Task<ISpecialismResultStrategy> GetSpecialismResultStrategyAsync(IEnumerable<TlLookup> tlLookup, int numberOfSpecialisms)
        {
            if (tlLookup == null)
                throw new ArgumentNullException(nameof(tlLookup), "The TlLookup cannot be null.");

            if (!tlLookup.Any())
                throw new ArgumentException("The TlLookup cannot be empty.", nameof(tlLookup));

          if (numberOfSpecialisms < 1 || numberOfSpecialisms > 2)
                throw new ArgumentException("The number of specialisms must be one or two.", nameof(numberOfSpecialisms));

            return GetSpecialismResultStrategy(tlLookup, numberOfSpecialisms);
        }

        private async Task<ISpecialismResultStrategy> GetSpecialismResultStrategy(IEnumerable<TlLookup> tlLookup, int numberOfSpecialisms)
        {
            bool found = _specialismStrategyDict.TryGetValue(numberOfSpecialisms, out ISpecialismResultStrategy specialismResultStrategy);

            if (found)
            {
                return specialismResultStrategy;
            }

            if (numberOfSpecialisms == 1)
            {
                specialismResultStrategy = new SingleSpecialismResultStrategy();
            }
            else
            {
                List<DualSpecialismOverallGradeLookup> dualSpecialismOverallGradeLookups = await GetDualSpecialismOverallGradeLookupData();
                specialismResultStrategy = new DualSpecialismResultStrategy(tlLookup, dualSpecialismOverallGradeLookups);
            }

            _specialismStrategyDict.Add(numberOfSpecialisms, specialismResultStrategy);
            return specialismResultStrategy;
        }

        private async Task<List<DualSpecialismOverallGradeLookup>> GetDualSpecialismOverallGradeLookupData()
        {
            return await _dualSpecialismOverallGradeLookupRepository.GetManyAsync(
                 navigationPropertyPath: new Expression<Func<DualSpecialismOverallGradeLookup, object>>[] {
                     o => o.FirstTlLookupSpecialismGrade,
                     o => o.SecondTlLookupSpecialismGrade,
                     o => o.TlLookupOverallSpecialismGrade
                 }).ToListAsync();
        }
    }
}