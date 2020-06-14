using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Models.BlobStorage
{
    public class BlobStorageData
    {
        public string ContainerName { get; set; }

        public string BlobFileName { get; set; }

        public string SourceFilePath { get; set; }

        public string DestinationFilePath { get; set; }

        public string UserName { get; set; }

        public Stream FileStream { get; set; }

        public byte[] FileData { get; set; }
    }
}
