using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlProviderAddressBuilder
    {
        public TlProviderAddress Build(TlProvider tlProvider = null)
        {
            return new TlProviderAddress
            {
                TlProvider = tlProvider ?? new TlProviderBuilder().Build(),
                DepartmentName = "Exams Office",
                OrganisationName = "Org name1",
                AddressLine1 = "Test Line 1",
                AddressLine2 = "Test Line 2",
                Town = "Test Town",
                Postcode = "xx1 1yy",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TlProviderAddress> BuildList(TlProvider tlProvider = null) => new List<TlProviderAddress>
        {
            new TlProviderAddress
            {
                TlProvider = tlProvider ?? new TlProviderBuilder().Build(),
                DepartmentName = "Exams Office",
                OrganisationName = "Org name1",
                AddressLine1 = "Test Line 1",
                AddressLine2 = "Test Line 2",
                Town = "Test Town",
                Postcode = "xx1 1yy",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new TlProviderAddress
            {
                TlProvider = tlProvider ?? new TlProviderBuilder().Build(),
                DepartmentName = "Test Office",
                OrganisationName = "Org name2",
                AddressLine1 = "Line 1",
                AddressLine2 = "Line 2",
                Town = "Town",
                Postcode = "aa5 1tt",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
