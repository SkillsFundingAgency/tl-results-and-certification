using Aspose.Pdf;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultSlipsGeneratorServiceBase
    {
        private readonly IBlobStorageService _blobStorageService;
        internal Document Document;
        private const string AsposeLicenceFilename = "aspose.pdf.net.lic";

        public ResultSlipsGeneratorServiceBase(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
            SetAsposeLicence();
        }

        private async void SetAsposeLicence()
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Aspose.ToString().ToLowerInvariant(),
                BlobFileName = AsposeLicenceFilename
            });

            if (fileStream == null || fileStream.Length == 0)
            {
                throw new Exception("Aspose licence not found");
            }
            License license = new();
            fileStream.Position = 0;
            license.SetLicense(fileStream);
        }
    }
}