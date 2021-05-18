using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlProviderBuilder 
    {
        public Domain.Models.TlProvider Build()
        {
            return new Domain.Models.TlProvider
            {
                UkPrn = 10000536,
                Name = "Barnsley College",
                DisplayName = "Barnsley College",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.TlProvider> BuildList() => new List<Domain.Models.TlProvider>
        {
            new Domain.Models.TlProvider
            {
                UkPrn = 10000536,
                Name = "Barnsley College",
                DisplayName = "Barnsley College",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlProvider
            {
                UkPrn = 10000721,
                Name = "Bishop Burton College",
                DisplayName = "Bishop Burton College",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlProvider
            {
                UkPrn = 10007315,
                Name = "Walsall College",
                DisplayName = "Walsall College",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlProvider
            {
                UkPrn = 10042313,
                Name = "Walsall Studio School",
                DisplayName = "Walsall Studio School",
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
