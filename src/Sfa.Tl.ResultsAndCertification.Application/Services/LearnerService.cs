using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ILearnerRepository> _logger;

        public LearnerService(ILearnerRepository learnerRepository, IMapper mapper, ILogger<ILearnerRepository> logger)
        {
            _learnerRepository = learnerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LearnerRecord> GetLearnerRecordAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistrationPathway = await _learnerRepository.GetLearnerRecordAsync(aoUkprn, profileId);
            if (tqRegistrationPathway == null || (status != null && tqRegistrationPathway.Status != status)) return null;

            var learnerRecord = _mapper.Map<LearnerRecord>(tqRegistrationPathway);

            return learnerRecord;
        }
    }
}
