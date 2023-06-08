using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class OverallGradeStrategyFactory
    {
        private readonly IRepository<TlLookup> _lookupRepo;
        private readonly IRepository<OverallResult> _overallResultRepo;

        private List<TlLookup> _tlLookup;



        public OverallGradeStrategyFactory(IRepository<TlLookup> lookupRepo, IRepository<OverallResult> overallResultRepo)
        {
            _lookupRepo = lookupRepo;
            _overallResultRepo = overallResultRepo;
        }

        public async Task<IOverallGradeStrategy> GetStrategy(int academicYear)
        {
            _tlLookup ??= await GetTlLookupData();

            //if (academicYear == 2020)
            //{

            //    return new OverallGradeStrategy2020(_tlLookup);
            //}
            //else
            //{
            //    return new OverallGradeStrategy2021
            //}

            throw new NotImplementedException();

        }

        private async Task<List<TlLookup>> GetTlLookupData()
        {
            return await _lookupRepo.GetManyAsync().ToListAsync();
        }
    }
}