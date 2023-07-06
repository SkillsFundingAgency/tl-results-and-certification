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
    public class OverallGradeStrategyFactory : IOverallGradeStrategyFactory
    {
        private readonly IRepository<OverallGradeLookup> _overallGradeLookupRepository;
        private readonly Dictionary<int, IOverallGradeStrategy> _overallGradeStrategyDict = new();

        public OverallGradeStrategyFactory(IRepository<OverallGradeLookup> overallResultRepository)
        {
            _overallGradeLookupRepository = overallResultRepository;
        }

        public async Task<IOverallGradeStrategy> GetOverallGradeStrategy(int academicYear, IEnumerable<TlLookup> tlLookup)
        {
            if (academicYear < 2020)
                throw new ArgumentException("The year must be greater than or equal to 2020.", nameof(academicYear));

            if (tlLookup == null)
                throw new ArgumentNullException(nameof(tlLookup), "The TlLookup cannot be null.");

            if (!tlLookup.Any())
                throw new ArgumentException("The TlLookup cannot be empty.", nameof(tlLookup));

            bool found = _overallGradeStrategyDict.TryGetValue(academicYear, out IOverallGradeStrategy overallGradeStrategy);

            if (found)
            {
                return overallGradeStrategy;
            }

            overallGradeStrategy = academicYear == 2020
                ? new OverallGradeStrategy2020(tlLookup, await GetOverallGradeLookupData(academicYear))
                : new OverallGradeStrategy2021Onwards(tlLookup, await GetOverallGradeLookupData(academicYear));

            _overallGradeStrategyDict.Add(academicYear, overallGradeStrategy);

            return overallGradeStrategy;
        }

        private async Task<List<OverallGradeLookup>> GetOverallGradeLookupData(int startYear)
        {
            var overallGradeLookup = await GetOverallGradeLookupData(p => p.StartYear == startYear);

            if (overallGradeLookup == null || !overallGradeLookup.Any())
            {
                overallGradeLookup = await GetOverallGradeLookupData(p => !p.StartYear.HasValue);
            }

            return overallGradeLookup;
        }

        private async Task<List<OverallGradeLookup>> GetOverallGradeLookupData(Expression<Func<OverallGradeLookup, bool>> condition)
        {
            return await _overallGradeLookupRepository.GetManyAsync(
                condition,
                new Expression<Func<OverallGradeLookup, object>>[] { o => o.TlLookupOverallGrade })
                .ToListAsync();
        }
    }
}