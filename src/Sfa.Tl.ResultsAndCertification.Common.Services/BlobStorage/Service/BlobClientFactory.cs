using Azure.Identity;
using Azure.Storage.Blobs;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly string _blobContainerUriTemplate;

        public BlobClientFactory(ResultsAndCertificationConfiguration config)
        {
            _blobContainerUriTemplate = config.BlobContainerUriTemplate;
        }

        public BlobClient Create(string containerName, string filePath, string fileName)
            => Create(containerName, $"{filePath}/{fileName}");

        public BlobClient Create(string containerName, string blobName)
        {
            BlobContainerClient blobContainerClient = CreateBlobContainerClient(containerName);
            return blobContainerClient.GetBlobClient(blobName.ToLowerInvariant());
        }

        private BlobContainerClient CreateBlobContainerClient(string containerName)
        {
            var blobContainerUri = new Uri(string.Format(_blobContainerUriTemplate, containerName.ToLowerInvariant()));

            var blobContainerClient = new BlobContainerClient(blobContainerUri, new DefaultAzureCredential());
            blobContainerClient.CreateIfNotExists();

            return blobContainerClient;
        }
    }
}