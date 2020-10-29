using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class DocumentUploadHistoryBuilder
    {
        public DocumentUploadHistory Build() => new DocumentUploadHistory
        {
            TlAwardingOrganisationId = 1,
            BlobUniqueReference = new Guid("F40C11CC-9411-4499-858E-A2D90B3F017A"),
            BlobFileName = "TlevelDetailsQueried",
            DocumentType = (int)DocumentType.Registrations,
            FileType = (int)FileType.Csv,
            Status = (int)BulkProcessStatus.Processed,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<DocumentUploadHistory> BuildList() => new List<DocumentUploadHistory>
        {
            new DocumentUploadHistory
            {
                TlAwardingOrganisationId = 1,
                BlobUniqueReference = new Guid("F40C11CC-9411-4499-858E-A2D90B3F017A"),
                BlobFileName = "inputfile_1.csv",
                DocumentType = (int)DocumentType.Registrations,
                FileType = (int)FileType.Csv,
                Status = (int)BulkProcessStatus.Processed,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new DocumentUploadHistory
            {
                TlAwardingOrganisationId = 1,
                BlobUniqueReference = new Guid("AD3463E7-4985-4219-BEAC-C88BFEB8CEF0"),
                BlobFileName = "inputfile_2.csv",
                DocumentType = (int)DocumentType.Registrations,
                FileType = (int)FileType.Csv,
                Status = (int)BulkProcessStatus.Processed,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
