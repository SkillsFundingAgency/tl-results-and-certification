using Azure.Storage.Blobs;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface
{
    public interface IBlobContainerClientFactory
    {
        BlobContainerClient Create(string containerName);
    }
}