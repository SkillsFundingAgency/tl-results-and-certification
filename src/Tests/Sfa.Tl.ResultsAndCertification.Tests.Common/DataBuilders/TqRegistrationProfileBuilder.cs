using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationProfileBuilder
    {
        public Domain.Models.TqRegistrationProfile Build() => new Domain.Models.TqRegistrationProfile
        {
            UniqueLearnerNumber = 1111111111,
            Firstname = "First 1",
            Lastname = "Last 1",
            DateofBirth = "10/10/1980".ToDateTime(),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqRegistrationProfile> BuildList() => new List<Domain.Models.TqRegistrationProfile>
        {
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111111,
                Firstname = "First 1",
                Lastname = "Last 1",
                DateofBirth = "10/10/1980".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111112,
                Firstname = "First 2",
                Lastname = "Last 2",
                DateofBirth = "07/05/1981".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationProfile
            {
                UniqueLearnerNumber = 1111111113,
                Firstname = "First 3",
                Lastname = "Last 3",
                DateofBirth = "03/07/1982".ToDateTime(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
