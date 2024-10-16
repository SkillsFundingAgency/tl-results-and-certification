using Azure.Storage.Blobs;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface
{
    public interface IBlobClientFactory
    {
        BlobClient Create(string containerName, string blobName);
        BlobClient Create(string containerName, string filePath, string fileName);
    }
}