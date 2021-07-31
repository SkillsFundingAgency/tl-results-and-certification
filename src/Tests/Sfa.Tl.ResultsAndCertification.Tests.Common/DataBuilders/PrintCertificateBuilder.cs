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
            tqRegistrationPathway = tqRegistrationPathway ?? new TqRegistrationPathwayBuilder().Build();
            return new PrintCertificate
            {
                PrintBatchItem = printBatchItem ?? new PrintBatchItemBuilder().Build(),
                TqRegistrationPathway = tqRegistrationPathway,
                Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                LearnerName = $"{tqRegistrationPathway.TqRegistrationProfile.Firstname} {tqRegistrationPathway.TqRegistrationProfile.Lastname}",
                Type = PrintCertificateType.StatementOfAchievement,
                LearningDetails = "Test",
                DisplaySnapshot = "Display",
                Status = PrintCertificateStatus.Created,                
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
                    Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    LearnerName = $"{tqRegistrationPathway.TqRegistrationProfile.Firstname} {tqRegistrationPathway.TqRegistrationProfile.Lastname}",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Test",
                    DisplaySnapshot = "Display",
                    Status = PrintCertificateStatus.Created,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new PrintCertificate
                {
                    PrintBatchItem = printBatchItem,
                    TqRegistrationPathway = tqRegistrationPathway,
                    Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    LearnerName = $"{tqRegistrationPathway.TqRegistrationProfile.Firstname} {tqRegistrationPathway.TqRegistrationProfile.Lastname}",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Details",
                    DisplaySnapshot = "Snapshot",
                    Status = PrintCertificateStatus.Created,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
        }

        public IList<PrintCertificate> BuildList(IList<TqRegistrationPathway> tqRegistrationPathway, PrintBatchItem printBatchItem = null)
        {
            printBatchItem ??= new PrintBatchItemBuilder().Build();
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().BuildList();

            return new List<PrintCertificate>
            {
                new PrintCertificate
                {
                    PrintBatchItem = printBatchItem,
                    TqRegistrationPathway = tqRegistrationPathway[0],
                    Uln = tqRegistrationPathway[0].TqRegistrationProfile.UniqueLearnerNumber,
                    LearnerName = $"{tqRegistrationPathway[0].TqRegistrationProfile.Firstname} {tqRegistrationPathway[0].TqRegistrationProfile.Lastname}",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Test",
                    DisplaySnapshot = "Display",
                    Status = PrintCertificateStatus.Created,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new PrintCertificate
                {
                    PrintBatchItem = printBatchItem,
                    TqRegistrationPathway = tqRegistrationPathway[1],
                    Uln = tqRegistrationPathway[1].TqRegistrationProfile.UniqueLearnerNumber,
                    LearnerName = $"{tqRegistrationPathway[1].TqRegistrationProfile.Firstname} {tqRegistrationPathway[1].TqRegistrationProfile.Lastname}",
                    Type = PrintCertificateType.StatementOfAchievement,
                    LearningDetails = "Details",
                    DisplaySnapshot = "Snapshot",
                    Status = PrintCertificateStatus.Created,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
        }
    }
}