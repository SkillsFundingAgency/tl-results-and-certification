using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class PrintCertificateBuilder
    {
        public PrintCertificate Build(PrintBatchItem printBatchItem = null, TqRegistrationPathway tqRegistrationPathway = null)
        {

            return new PrintCertificate
            {
                PrintBatchItem = printBatchItem ?? new PrintBatchItemBuilder().Build(),
                TqRegistrationPathway = tqRegistrationPathway ?? new TqRegistrationPathwayBuilder().Build(),
                Uln = 1111111111,
                LearnerName = "First 1 Last 1",
                Type = PrintCertificateType.StatementOfAchievement,
                LearningDetails = "Test",
                DisplaySnapshot = "Display",
                Status = PrintCertificateStatus.Created,
                Reason = string.Empty,
                TrackingId = "TR12345687",
                StatusChangedOn = DateTime.UtcNow,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<PrintCertificate> BuildList(PrintBatchItem printBatchItem = null, TqRegistrationPathway tqRegistrationPathway = null)
        {
            printBatchItem ??= new PrintBatchItemBuilder().Build();
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();

            return new List<PrintCertificate> 
            {
                new PrintCertificate
                {
                    PrintBatchItem = printBatchItem,
                    TqRegistrationPathway = tqRegistrationPathway,
                    Uln = 1111111111,
                    LearnerName = "First 1 Last 1",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Test",
                    DisplaySnapshot = "Display",
                    Status = PrintCertificateStatus.Created,
                    Reason = string.Empty,
                    TrackingId = "TR12345687",
                    StatusChangedOn = DateTime.UtcNow,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new PrintCertificate
                {
                    PrintBatchItem = printBatchItem,
                    TqRegistrationPathway = tqRegistrationPathway,
                    Uln = 1111111112,
                    LearnerName = "First 2 Last 2",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Details",
                    DisplaySnapshot = "Snapshot",
                    Status = PrintCertificateStatus.Created,
                    Reason = string.Empty,
                    TrackingId = "UR85697458",
                    StatusChangedOn = DateTime.UtcNow,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
        }
    }
}
