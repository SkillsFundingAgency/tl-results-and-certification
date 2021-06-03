using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class BatchBuilder
    {
        public Domain.Models.Batch Build()
        {
            return new Domain.Models.Batch
            {
                Type = BatchType.Printing,
                Status = BatchStatus.Created,
                RunOn = DateTime.UtcNow,
                StatusChangedOn = DateTime.UtcNow.AddDays(1),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.Batch> BuildList()
        {
            return new List<Domain.Models.Batch> 
            {
                new Domain.Models.Batch
                {
                    Type = BatchType.Printing,
                    Status = BatchStatus.Created,
                    RunOn = DateTime.UtcNow,
                    StatusChangedOn = DateTime.UtcNow.AddDays(1),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.Batch
                {
                    Type = BatchType.Printing,
                    Status = BatchStatus.Created,
                    RunOn = DateTime.UtcNow,
                    StatusChangedOn = DateTime.UtcNow.AddDays(1),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
        }
    }
}
