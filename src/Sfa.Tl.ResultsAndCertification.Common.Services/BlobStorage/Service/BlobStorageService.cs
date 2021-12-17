using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Collections.Generic;
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
            var blobClient = await GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            await blobClient.UploadAsync(blobStorageData.FileStream);
        }

        public async Task UploadFromByteArrayAsync(BlobStorageData blobStorageData)
        {
            var blobClient = await GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            //await blobClient.SetMetadataAsync(new Dictionary<string, string> { { "RequestedBy", blobStorageData.UserName } }); // TODO.
            
            await using var fileStream = new MemoryStream(blobStorageData.FileData);
            await blobClient.UploadAsync(fileStream);
            fileStream.Close();
        }

        public async Task<Stream> DownloadFileAsync(BlobStorageData blobStorageData)
        {
            var blobClient = await GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            if (await blobClient.ExistsAsync())
            {
                var ms = new MemoryStream();
                await blobClient.DownloadToAsync(ms);
                return ms;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> MoveFileAsync(BlobStorageData blobStorageData)
        {
            var sourceBlobClient = await GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            if(await sourceBlobClient.ExistsAsync())
            {
                var lease = sourceBlobClient.GetBlobLeaseClient();
                await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                var destinationBlobClient = await GetBlobClient(blobStorageData.ContainerName, blobStorageData.DestinationFilePath, blobStorageData.BlobFileName);

                await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);

                var sourceBlobClientProperties = await sourceBlobClient.GetPropertiesAsync();

                if (sourceBlobClientProperties.Value.LeaseState == LeaseState.Leased)
                    await lease.BreakAsync();

                return await sourceBlobClient.DeleteIfExistsAsync();
            }
            return false;
        }   

        public async Task<bool> DeleteFileAsync(BlobStorageData blobStorageData)
        {
            var blobClient= await GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            return await blobClient.DeleteIfExistsAsync();
        }

        private async Task<BlobClient> GetBlobClient(string containerName, string filePath, string fileName)
        {
            var blobContainerClient = await GetContainerAsync(containerName);
            var blobFileReference = !string.IsNullOrWhiteSpace(filePath) ? $"{filePath}/{fileName}" : fileName;
            var blobClient = blobContainerClient.GetBlobClient(blobFileReference?.ToLowerInvariant());
            return blobClient;
        }

        private async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_configuration.BlobStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName?.ToLowerInvariant());

            if (!await containerClient.ExistsAsync())
                await containerClient.CreateAsync();

            return containerClient;
        }
    }
}
