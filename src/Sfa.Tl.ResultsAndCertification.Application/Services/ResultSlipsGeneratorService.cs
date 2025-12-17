using Aspose.Pdf;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Sfa.Tl.ResultsAndCertification.Application.Services.ResultSlipsBuilder.ResultSlipsGeneratorService;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public partial class ResultSlipsGeneratorService : ResultSlipsGeneratorServiceBase, IResultSlipsGeneratorService
    {
        private readonly ISystemProvider _systemProvider;

        public ResultSlipsGeneratorService(IBlobStorageService blobStorageService,
            ILogger<IResultSlipsGeneratorService> logger,
            ISystemProvider systemProvider) : base(blobStorageService, logger)
        {
            _systemProvider = systemProvider;
        }

        public byte[] GetByteData(IEnumerable<DownloadOverallResultSlipsData> data)
        {
            Document = new();
            MemoryStream stream = new();

            if (data == null || !data.Any())
            {
                Document.Pages.Add();
                Document.Save(stream);
                return stream.ToArray();
            }

            foreach (DownloadOverallResultSlipsData item in data)
            {
                var page = Document.Pages.Add();
                page.PageInfo.Margin = PageMargins;

                page = ResultSlipBuilder.Create(item, page, _systemProvider)
                                        .AddHeader()
                                        .AddHeaderImage()
                                        .AddLearnerTable()
                                        .AddResultsTable()
                                        .AddFooter()
                                        .Build();
            }

            Document.Save(stream);
            return stream.ToArray();
        }

        public MarginInfo PageMargins => new MarginInfo(40, 40, 40, 40);
    }
}