using System;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IpLookupBuilder
    {
        public Domain.Models.IpLookup Build()
        {
            var tlLookup = new TlLookupBuilder().Build();
            return new Domain.Models.IpLookup
            {
                TlLookupId = tlLookup.Id,
                Name = "Learner's medical reasons",
                StartDate = DateTime.Today,
                EndDate = null,
                ShowOption = null,
                SortOrder = 1,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.IpLookup> BuildList()
        {
            var tlLookup = new TlLookupBuilder().Build();

            var ipLookupValues = new List<Domain.Models.IpLookup>()
            {
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Learner's medical reasons",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 1,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Learner's family medical reasons",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 2,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Bereavement",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 3,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Domestic crisis",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 4,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Domestic crisis",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 5,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Trauma or significant change of circumstances",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 6,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Unsafe placement",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 7,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Placement withdrawn",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 8,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpLookup
                {
                    TlLookupId = tlLookup.Id,
                    Name = "Covid-19",
                    StartDate = DateTime.Today,
                    EndDate = null,
                    ShowOption = null,
                    SortOrder = 9,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return ipLookupValues;
        }
    }
}
