using Azure.Identity;
using Azure.Storage.Blobs;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobClientFactory : IBlobClientFactory
    {
        private const string LocalConnectionString = "UseDevelopmentStorage=true;";

        private readonly string _blobContainerUriTemplate;
        private readonly bool _isDevelopment;

        public BlobClientFactory(string blobContainerUriTemplate, bool isDevelopment)
        {
            _blobContainerUriTemplate = blobContainerUriTemplate;
            _isDevelopment = isDevelopment;
        }

        public BlobClient Create(string containerName, string filePath, string fileName)
            => Create(containerName, $"{filePath}/{fileName}");

        public BlobClient Create(string containerName, string blobName)
        {
            BlobContainerClient blobContainerClient = _isDevelopment ? CreateLocalBlobContainerClient(containerName) : CreateBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();

            return blobContainerClient.GetBlobClient(blobName.ToLowerInvariant());
        }

        private BlobContainerClient CreateBlobContainerClient(string containerName)
        {
            var blobContainerUri = new Uri(string.Format(_blobContainerUriTemplate, containerName.ToLowerInvariant()));
            var blobContainerClient = new BlobContainerClient(blobContainerUri, new DefaultAzureCredential());

            return blobContainerClient;
        }

        private static BlobContainerClient CreateLocalBlobContainerClient(string containerName)
            => new(LocalConnectionString, containerName.ToLowerInvariant());
    }
}