using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DocumentUploadHistoryService : IDocumentUploadHistoryService
    {
        private readonly IRepository<DocumentUploadHistory> _documentUploadHistoryRepository;
        private readonly IMapper _mapper;

        public DocumentUploadHistoryService(IRepository<DocumentUploadHistory> documentUploadHistoryRepository, IMapper mapper)
        {
            _documentUploadHistoryRepository = documentUploadHistoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateDocumentUploadHistory(DocumentUploadHistoryDetails model)
        {
            if (model != null)
            {
                var entityModel = _mapper.Map<DocumentUploadHistory>(model);
                return await _documentUploadHistoryRepository.CreateAsync(entityModel) > 0;
            }
            return false;
        }
    }
}
