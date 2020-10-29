using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Loader
{
    public class BulkBaseLoader : IBulkBaseLoader
    {
        private readonly IBlobStorageService _blobStorageService;

        public BulkBaseLoader(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> DeleteFileFromProcessingFolderAsync(BulkRegistrationRequest request)
        {
            if (request == null) return false;

            await _blobStorageService.DeleteFileAsync(new BlobStorageData
            {
                ContainerName = request.DocumentType.ToString(),
                SourceFilePath = $"{request.AoUkprn}/{BulkRegistrationProcessStatus.Processing}",
                BlobFileName = request.BlobFileName
            });
            return true;
        }
    }
}
