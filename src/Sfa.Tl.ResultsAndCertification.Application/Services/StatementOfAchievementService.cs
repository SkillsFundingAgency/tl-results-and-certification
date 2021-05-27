using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class StatementOfAchievementService : IStatementOfAchievementService
    {
        private readonly IStatementOfAchievementRepository _statementOfAchievementRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public StatementOfAchievementService(IStatementOfAchievementRepository statementOfAchievementRepository, IMapper mapper, ILogger<StatementOfAchievementService> logger)
        {
            _statementOfAchievementRepository = statementOfAchievementRepository;
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
    }
}
