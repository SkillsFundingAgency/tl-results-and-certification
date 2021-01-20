using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class TlLookupDataProvider
    {
        public static TlLookup CreateTlLookup(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlLookup = new TlLookupBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlLookup);
            }
            return tlLookup;
        }

        public static TlLookup CreateTlLookup(ResultsAndCertificationDbContext _dbContext, TlLookup tlLookup, bool addToDbContext = true)
        {
            if (tlLookup == null)
            {
                tlLookup = new TlLookupBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlLookup);
            }
            return tlLookup;
        }

        public static TlLookup CreateTlLookup(ResultsAndCertificationDbContext _dbContext, string category, string code, string value, bool isActive, bool addToDbContext = true)
        {
            var tlLookup = new TlLookup
            {
                Category = category,
                Code = code,
                Value = value,
                IsActive = isActive,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlLookup);
            }
            return tlLookup;
        }

        public static IList<TlLookup> CreateTlLookupList(ResultsAndCertificationDbContext _dbContext, IList<TlLookup> tlLookup = null, bool addToDbContext = true)
        {
            if (tlLookup == null)
                tlLookup = new TlLookupBuilder().BuildList();

            if (addToDbContext)
            {
                _dbContext.AddRange(tlLookup);
            }
            return tlLookup;
        }
    }
}
