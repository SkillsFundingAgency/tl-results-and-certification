using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
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