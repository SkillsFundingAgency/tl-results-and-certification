using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IDocumentUploadHistoryService
    {
        Task<bool> CreateDocumentUploadHistory(DocumentUploadHistoryDetails model);
        Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetails(long aoUkprn, Guid blobUniqueReference);
    }
}
