using Aspose.Pdf;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultSlipsGeneratorServiceBase
    {
        private readonly IBlobStorageService _blobStorageService;
        protected readonly ILogger<IResultSlipsGeneratorService> _logger;

        internal Document Document;
        private const string AsposeLicenceFilename = "aspose.pdf.net.lic";

        public ResultSlipsGeneratorServiceBase(IBlobStorageService blobStorageService, ILogger<IResultSlipsGeneratorService> logger)
        {
            _blobStorageService = blobStorageService;
            _logger = logger;

            //SetAsposeLicence();
        }

        private async void SetAsposeLicence()
        {
            var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
            {
                ContainerName = DocumentType.Aspose.ToString(),
                BlobFileName = AsposeLicenceFilename
            });

            if (fileStream == null || fileStream.Length == 0)
            {
                _logger.LogError($"Unable to load Aspose licence from the storage account. Filename: {AsposeLicenceFilename}");
                return;
            }
            License license = new();
            fileStream.Position = 0;
            license.SetLicense(fileStream);
        }
    }
}