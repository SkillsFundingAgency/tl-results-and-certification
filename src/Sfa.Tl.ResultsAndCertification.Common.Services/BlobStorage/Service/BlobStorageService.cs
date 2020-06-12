using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Service
{
    public class BlobStorageService
    {
        private async Task<CloudBlockBlob> GetBlockBlobReference(string storageConnectionString, string containerName, string fileName)
        {
            var blobContainer = await GetContainerReferenceAsync(containerName, storageConnectionString);
            return blobContainer.GetBlockBlobReference(fileName);
        }

        private async Task<CloudBlobContainer> GetContainerReferenceAsync(string containerName, string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            await containerReference.CreateIfNotExistsAsync();
            return containerReference;
        }
    }
}
