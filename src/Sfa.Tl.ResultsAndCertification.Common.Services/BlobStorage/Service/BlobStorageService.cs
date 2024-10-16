using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IBlobClientFactory _blobClientFactory;

        public BlobStorageService(IBlobClientFactory blobClientFactory)
        {
            _blobClientFactory = blobClientFactory;
        }

        public async Task UploadFileAsync(BlobStorageData blobStorageData)
        {
            var blobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            await blobClient.UploadAsync(blobStorageData.FileStream);
        }

        public async Task UploadFromByteArrayAsync(BlobStorageData blobStorageData)
        {
            var blobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            var metadata = new Dictionary<string, string>
            {
                { Helpers.Constants.CreatedBy, blobStorageData.UserName ?? Helpers.Constants.DefaultPerformedBy }
            };

            await using var fileStream = new MemoryStream(blobStorageData.FileData);
            await blobClient.UploadAsync(fileStream, metadata: metadata);
            fileStream.Close();
        }

        public async Task<Stream> DownloadFileAsync(BlobStorageData blobStorageData)
        {
            var blobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            if (!await blobClient.ExistsAsync())
            {
                return null;
            }

            var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);

            return ms;
        }

        public async Task<bool> MoveFileAsync(BlobStorageData blobStorageData)
        {
            var sourceBlobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);

            if (!await sourceBlobClient.ExistsAsync())
            {
                return false;
            }

            var lease = sourceBlobClient.GetBlobLeaseClient();
            await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

            var destinationBlobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.DestinationFilePath, blobStorageData.BlobFileName);

            await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);

            var sourceBlobClientProperties = await sourceBlobClient.GetPropertiesAsync();

            if (sourceBlobClientProperties.Value.LeaseState == LeaseState.Leased)
                await lease.BreakAsync();

            return await sourceBlobClient.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteFileAsync(BlobStorageData blobStorageData)
        {
            var blobClient = GetBlobClient(blobStorageData.ContainerName, blobStorageData.SourceFilePath, blobStorageData.BlobFileName);
            return await blobClient.DeleteIfExistsAsync();
        }

        private BlobClient GetBlobClient(string containerName, string filePath, string fileName)
            => _blobClientFactory.Create(containerName, filePath, fileName);
    }
}