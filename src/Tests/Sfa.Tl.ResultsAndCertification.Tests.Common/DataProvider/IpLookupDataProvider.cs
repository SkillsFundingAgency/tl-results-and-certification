using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;


namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class IpLookupDataProvider
    {
        public static IpLookup CreateIpLookup(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var IpLookup = new IpLookupBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(IpLookup);
            }
            return IpLookup;
        }

        public static IpLookup CreateIpLookup(ResultsAndCertificationDbContext _dbContext, IpLookup ipLookup, bool addToDbContext = true)
        {
            if (ipLookup == null)
            {
                ipLookup = new IpLookupBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(ipLookup);
            }
            return ipLookup;
        }

        public static IpLookup CreateIpLookup(ResultsAndCertificationDbContext _dbContext, int tlLookupId, string name, DateTime startDate, DateTime? endDate, int? showOption, int? sortOrder, bool addToDbContext = true)
        {
            var ipLookup = new IpLookup
            {
                TlLookupId = tlLookupId,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                ShowOption = showOption,
                SortOrder = sortOrder,
                CreatedBy = "Test User"
            };

            if (addToDbContext)
            {
                _dbContext.Add(ipLookup);
            }
            return ipLookup;
        }

        public static IList<IpLookup> CreateIpLookupList(ResultsAndCertificationDbContext _dbContext, IList<IpLookup> ipLookup = null, IpLookupType? ipLookupType = null, bool addToDbContext = true)
        {
            if (ipLookup == null)
                ipLookup = new IpLookupBuilder().BuildList(ipLookupType);

            if (addToDbContext)
            {
                _dbContext.AddRange(ipLookup);
            }
            return ipLookup;
        }
    }
}
