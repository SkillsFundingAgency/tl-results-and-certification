using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DocumentUploadHistoryService : IDocumentUploadHistoryService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<IRepository<DocumentUploadHistory>> _logger;
        private readonly IRepository<DocumentUploadHistory> _documentUploadHistoryRepository;
        private readonly IRepository<TlAwardingOrganisation> _tlAwardingOrganisationRepository;
        private readonly IRepository<TlProvider> _tlProviderRepository;

        public DocumentUploadHistoryService(ILogger<IRepository<DocumentUploadHistory>> logger, IMapper mapper, IRepository<DocumentUploadHistory> documentUploadHistoryRepository,
            IRepository<TlAwardingOrganisation> tlAwardingOrganisationRepository, IRepository<TlProvider> tlProviderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _documentUploadHistoryRepository = documentUploadHistoryRepository;
            _tlAwardingOrganisationRepository = tlAwardingOrganisationRepository;
            _tlProviderRepository = tlProviderRepository;
        }

        public async Task<bool> CreateDocumentUploadHistory(DocumentUploadHistoryDetails model)
        {
            if (model != null)
            {
                if (model.LoginUserType == LoginUserType.AwardingOrganisation)
                {
                    var tlAwardingOrganisation = await _tlAwardingOrganisationRepository.GetFirstOrDefaultAsync(x => x.UkPrn == model.Ukprn);
                    if (tlAwardingOrganisation == null)
                    {
                        _logger.LogWarning(LogEvent.NoDataFound, $"TlAwardingOrganisationId not found for Ukprn = {model.Ukprn}. Method: CreateDocumentUploadHistory()");
                        return false;
                    }
                    model.TlAwardingOrganisationId = tlAwardingOrganisation.Id;
                }
                else if (model.LoginUserType == LoginUserType.TrainingProvider)
                {
                    var tlProvider = await _tlProviderRepository.GetFirstOrDefaultAsync(x => x.UkPrn == model.Ukprn);
                    if (tlProvider == null)
                    {
                        _logger.LogWarning(LogEvent.NoDataFound, $"TlProviderId not found for Ukprn = {model.Ukprn}. Method: CreateDocumentUploadHistory()");
                        return false;
                    }
                    model.TlProviderId = tlProvider.Id;
                }
                else
                {
                    throw new ApplicationException("LoginUserType need to be specified");
                }

                var entityModel = _mapper.Map<DocumentUploadHistory>(model);
                return await _documentUploadHistoryRepository.CreateAsync(entityModel) > 0;
            }
            return false;
        }

        public async Task<DocumentUploadHistoryDetails> GetDocumentUploadHistoryDetails(long ukprn, Guid blobUniqueReference)
        {
            var model = await _documentUploadHistoryRepository
                .GetFirstOrDefaultAsync(x => x.BlobUniqueReference == blobUniqueReference && (x.TlAwardingOrganisation.UkPrn == ukprn || x.TlProvider.UkPrn == ukprn));

            return _mapper.Map<DocumentUploadHistoryDetails>(model);
        }
    }
}
