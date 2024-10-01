using Azure.Identity;
using Azure.Storage.Blobs;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobContainerClientFactory : IBlobContainerClientFactory
    {
        private readonly string _blobContainerUriTemplate;

        public BlobContainerClientFactory(ResultsAndCertificationConfiguration config)
        {
            _blobContainerUriTemplate = config.BlobContainerUriTemplate;
        }

        public BlobContainerClient Create(string containerName)
        {
            var blobContainerUri = new Uri(string.Format(_blobContainerUriTemplate, containerName.ToLowerInvariant()));

            var blobContainerClient = new BlobContainerClient(blobContainerUri, new DefaultAzureCredential());
            blobContainerClient.CreateIfNotExists();

            return blobContainerClient;
        }
    }
}