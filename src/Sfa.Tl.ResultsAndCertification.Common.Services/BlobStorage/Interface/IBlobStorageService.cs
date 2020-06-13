using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(BlobStorageData blobStorageData);
        Task<Stream> DownloadFileAsync(BlobStorageData blobStorageData);
        Task<bool> MoveFileAsync(BlobStorageData blobStorageData);
    }
}
