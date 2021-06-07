using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class StatementOfAchievementService : IStatementOfAchievementService
    {
        private readonly IStatementOfAchievementRepository _statementOfAchievementRepository;
        private readonly IRepository<Batch> _batchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public StatementOfAchievementService(IStatementOfAchievementRepository statementOfAchievementRepository, IRepository<Batch> batchRepository, IMapper mapper, ILogger<StatementOfAchievementService> logger)
        {
            _statementOfAchievementRepository = statementOfAchievementRepository;
            _batchRepository = batchRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _statementOfAchievementRepository.FindSoaLearnerRecordAsync(providerUkprn, uln);
        }

        public async Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            return await _statementOfAchievementRepository.GetSoaLearnerRecordDetailsAsync(providerUkprn, profileId);
        }

        public async Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(SoaPrintingRequest request)
        {
            var soaBatchRequest = _mapper.Map<Batch>(request);
            var result = await _batchRepository.CreateAsync(soaBatchRequest);
            return new SoaPrintingResponse { Uln = request.Uln, LearnerName = request.LearnerName, IsSuccess = result > 0 };
        }

        public async Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId)
        {
            return await _statementOfAchievementRepository.GetPrintRequestSnapshotAsync(providerUkprn, profileId, pathwayId);
        }
    }
}