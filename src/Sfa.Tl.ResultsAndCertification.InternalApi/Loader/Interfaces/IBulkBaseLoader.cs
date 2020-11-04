using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IBulkBaseLoader
    {
        public Task<bool> DeleteFileFromProcessingFolderAsync(BulkProcessRequest request);
        public Task<bool> UploadErrorsFileToBlobStorage(BulkProcessRequest request, byte[] errorFile);
        public Task<bool> MoveFileFromProcessingToFailedAsync(BulkProcessRequest request);
        public Task<bool> MoveFileFromProcessingToProcessedAsync(BulkProcessRequest request);
        public Task<bool> CreateDocumentUploadHistory(BulkProcessRequest request, DocumentUploadStatus status = DocumentUploadStatus.Processed);
    }
}
