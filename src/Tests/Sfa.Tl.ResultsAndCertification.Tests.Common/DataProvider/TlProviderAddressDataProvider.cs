using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class TlProviderAddressDataProvider
    {
        public static TlProviderAddress CreateTlProviderAddress(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlAwardingOrganisation = new TlProviderAddressBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlAwardingOrganisation);
            }
            return tlAwardingOrganisation;
        }

        public static TlProviderAddress CreateTlProviderAddress(ResultsAndCertificationDbContext _dbContext, TlProviderAddress tlProviderAddress, bool addToDbContext = true)
        {
            if (tlProviderAddress == null)
            {
                tlProviderAddress = new TlProviderAddressBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlProviderAddress);
            }
            return tlProviderAddress;
        }
        public static IList<TlProviderAddress> CreateTlProviderAddress(ResultsAndCertificationDbContext _dbContext, IList<TlProviderAddress> tlProviderAddresses, bool addToDbContext = true)
        {
            if (addToDbContext && tlProviderAddresses != null && tlProviderAddresses.Count > 0)
            {
                _dbContext.AddRange(tlProviderAddresses);
            }
            return tlProviderAddresses;
        }


        public static TlProviderAddress CreateTlProviderAddress(ResultsAndCertificationDbContext _dbContext, int tlProviderId, string departmentName, string organisationName, string addressLine1, string addressLine2, string town, string postcode, bool addToDbContext = true)
        {
            var tlProviderAddress = new TlProviderAddress
            {
                TlProviderId = tlProviderId,
                DepartmentName = departmentName,
                OrganisationName = organisationName,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                Town = town,
                Postcode = postcode
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlProviderAddress);
            }
            return tlProviderAddress;
        }
    }
}
