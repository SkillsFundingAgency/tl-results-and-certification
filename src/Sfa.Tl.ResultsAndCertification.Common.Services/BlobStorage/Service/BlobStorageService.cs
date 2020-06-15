using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;

        public BlobStorageService(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadFileAsync(BlobStorageData blobStorageData)
        {
            var blobReference = await GetBlockBlobReference(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            await blobReference.UploadFromStreamAsync(blobStorageData.FileStream);
        }

        public async Task UploadFromByteArrayAsync(BlobStorageData blobStorageData)
        {
            var blobReference = await GetBlockBlobReference(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            await blobReference.UploadFromByteArrayAsync(blobStorageData.FileData, 0, blobStorageData.FileData.Length);
        }

        public async Task<Stream> DownloadFileAsync(BlobStorageData blobStorageData)
        {
            var blobReference = await GetBlockBlobReference(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            if (await blobReference.ExistsAsync())
            {
                var ms = new MemoryStream();
                await blobReference.DownloadToStreamAsync(ms);
                return ms;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> MoveFileAsync(BlobStorageData blobStorageData)
        {
            var sourceBlobReference = await GetBlockBlobReference(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            var destinationBlobReference = await GetBlockBlobReference(blobStorageData.ContainerName, blobStorageData.DestinationFilePath, blobStorageData.BlobFileName);

            await destinationBlobReference.StartCopyAsync(sourceBlobReference);
            return await sourceBlobReference.DeleteIfExistsAsync();
        }        

        private async Task<CloudBlockBlob> GetBlockBlobReference(string containerName, string filePath, string fileName)
        {
            var blobContainer = await GetContainerReferenceAsync(containerName);
            var blobFileReference = !string.IsNullOrWhiteSpace(filePath) ? $"{filePath}/{fileName}" : fileName;
            return blobContainer.GetBlockBlobReference(blobFileReference?.ToLowerInvariant());
        }

        private async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName?.ToLowerInvariant());
            await containerReference.CreateIfNotExistsAsync();
            return containerReference;
        }
    }
}
