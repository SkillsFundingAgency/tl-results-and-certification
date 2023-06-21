using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class FunctionLogBuilder
    {
        private readonly DateTime CurrentDate = DateTime.Now.Date;

        public FunctionLog Build() => new FunctionLog
        {
            FunctionType = FunctionType.LearnerVerificationAndLearningEvents,
            Name = "LearningEvents",
            StartDate = CurrentDate.AddDays(1),
            EndDate = CurrentDate.AddDays(1).AddMinutes(10),
            Status = FunctionStatus.Processed,
            Message = "Processed Successfully",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<FunctionLog> BuildList() => new List<FunctionLog>
        {
            new FunctionLog
            {
                FunctionType = FunctionType.LearnerVerificationAndLearningEvents,
                Name = "LearningEvents",
                StartDate = CurrentDate.AddDays(1),
                EndDate = CurrentDate.AddDays(1).AddMinutes(10),
                Status = FunctionStatus.Processed,
                Message = "Processed Successfully", 
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new FunctionLog
            {
                FunctionType = FunctionType.LearnerVerificationAndLearningEvents,
                Name = "VerifyLearner",
                StartDate = CurrentDate.AddDays(1),
                EndDate = CurrentDate.AddDays(1).AddMinutes(10),
                Status = FunctionStatus.Processing,
                Message = "Started Processing",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new FunctionLog
            {
                FunctionType = FunctionType.OverallResultCalculation,
                Name = "CalculateResults",
                StartDate = CurrentDate.AddDays(1),
                EndDate = CurrentDate.AddDays(1).AddMinutes(10),
                Status = FunctionStatus.Processed,
                Message = "Processed Successfully",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new FunctionLog
            {
                FunctionType = FunctionType.UcasTransferEntries,
                Name = "Test",
                StartDate = CurrentDate.AddDays(1),
                EndDate = CurrentDate.AddDays(1).AddMinutes(10),
                Status = FunctionStatus.Failed,
                Message = "Job Failed",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new FunctionLog
            {
                FunctionType = FunctionType.IndustryPlacementExtract,
                Name = "IndustryPlacementExtract",
                StartDate = CurrentDate.AddDays(1),
                EndDate = CurrentDate.AddDays(1).AddMinutes(10),
                Status = FunctionStatus.Processed,
                Message = "Processed Successfully",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };
    }
}
