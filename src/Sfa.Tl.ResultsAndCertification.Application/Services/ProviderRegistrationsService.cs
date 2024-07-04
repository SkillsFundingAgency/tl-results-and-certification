using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ProviderRegistrationsService : IProviderRegistrationsService
    {
        private readonly IProviderRegistrationsRepository _repository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;

        public ProviderRegistrationsService(
            IProviderRegistrationsRepository repository,
            IBlobStorageService blobStorageService,
            IMapper mapper)
        {
            _repository = repository;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
        }

        public Task<IList<int>> GetAvailableStartYearsAsync(Func<DateTime> getToday)
            => _repository.GetAvailableStartYearsAsync(getToday);

        public async Task<DataExportResponse> GetRegistrationsAsync(long providerUkprn, int startYear, string requestedBy, Func<Guid> newGuid)
        {
            IList<ProviderRegistrationExport> registrations = await GetDownloadRegistrationsRecordsAsync(providerUkprn, startYear);

            if (registrations.IsNullOrEmpty())
            {
                return new DataExportResponse { IsDataFound = false };
            }

            byte[] byteData = await CsvExtensions.WriteFileAsync(registrations, classMapType: typeof(ProviderRegistrationExportMap));

            Guid blobUniqueReference = newGuid();
            await WriteCsvToBlobAsync(blobUniqueReference, providerUkprn, requestedBy, byteData);

            return new DataExportResponse
            {
                FileSize = Math.Round(byteData.Length / 1024D, 2),
                BlobUniqueReference = blobUniqueReference,
                ComponentType = ComponentType.NotSpecified,
                IsDataFound = true
            };
        }

        private async Task<IList<ProviderRegistrationExport>> GetDownloadRegistrationsRecordsAsync(long providerUkprn, int startYear)
        {
            IList<TqRegistrationPathway> pathways = await _repository.GetRegistrationsAsync(providerUkprn, startYear);
            return _mapper.Map<IList<ProviderRegistrationExport>>(pathways);
        }

        private Task WriteCsvToBlobAsync(Guid blobUniqueReference, long providerUkprn, string requestedBy, byte[] byteData)
            => _blobStorageService.UploadFromByteArrayAsync(new BlobStorageData
            {
                ContainerName = DocumentType.DataExports.ToString(),
                SourceFilePath = $"{providerUkprn}/{DataExportType.ProviderRegistrations}",
                BlobFileName = $"{blobUniqueReference}.{FileType.Csv}",
                FileData = byteData,
                UserName = requestedBy
            });
    }
}