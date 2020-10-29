using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces
{
    public interface IBulkBaseLoader
    {
        public Task<bool> DeleteFileFromProcessingFolderAsync(BulkRegistrationRequest request);
        public Task<byte[]> CreateErrorFileAsync(IList<RegistrationValidationError> validationErrors);
        public Task<bool> UploadErrorsFileToBlobStorage(BulkRegistrationRequest request, byte[] errorFile);
        public Task<bool> MoveFileFromProcessingToFailedAsync(BulkRegistrationRequest request);
        public Task<bool> CreateDocumentUploadHistory(BulkRegistrationRequest request, DocumentUploadStatus status = DocumentUploadStatus.Processed);
    }
}
