using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DocumentUploadHistoryService : IDocumentUploadHistoryService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<DocumentUploadHistory>> _logger;
        private readonly IRepository<DocumentUploadHistory> _documentUploadHistoryRepository;
        private readonly IRepository<TlAwardingOrganisation> _tlAwardingOrganisationRepository;

        public DocumentUploadHistoryService(ILogger<IRepository<DocumentUploadHistory>> logger, IMapper mapper, IRepository<DocumentUploadHistory> documentUploadHistoryRepository,
            IRepository<TlAwardingOrganisation> tlAwardingOrganisationRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _documentUploadHistoryRepository = documentUploadHistoryRepository;
            _tlAwardingOrganisationRepository = tlAwardingOrganisationRepository;           
        }

        public async Task<bool> CreateDocumentUploadHistory(DocumentUploadHistoryDetails model)
        {
            if (model != null)
            {
                var tlAwardingOrganisation = await _tlAwardingOrganisationRepository.GetFirstOrDefaultAsync(x => x.UkPrn == model.AoUkprn);

                if(tlAwardingOrganisation == null)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"TlAwardingOrganisationId not found for AoUkprn = {model.AoUkprn}. Method: CreateDocumentUploadHistory()");
                    return false;
                }
                model.TlAwardingOrganisationId = tlAwardingOrganisation.Id;
                var entityModel = _mapper.Map<DocumentUploadHistory>(model);
                return await _documentUploadHistoryRepository.CreateAsync(entityModel) > 0;
            }
            return false;
        }

        public async Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetails(long aoUkprn, Guid blobUniqueReference)
        {
            var model = await _documentUploadHistoryRepository
                .GetFirstOrDefaultAsync(x => x.BlobUniqueReference == blobUniqueReference && x.TlAwardingOrganisation.UkPrn == aoUkprn);

            return _mapper.Map<DocumentUploadHistoryDetails>(model);
        }
    }
}
