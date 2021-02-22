using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class LearnerRecordService : ILearnerRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRegistrationRepository _tqRegistrationRepository;

        public LearnerRecordService(IMapper mapper, ILogger<LearnerRecordService> logger, IRegistrationRepository tqRegistrationRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _tqRegistrationRepository = tqRegistrationRepository;
        }

        public async Task<IList<RegistrationLearnerDetails>> GetValidRegistrationLearners()
        {
            var registrationLearners = await _tqRegistrationRepository.GetManyAsync(r => r.IsLearnerVerified == null || r.IsLearnerVerified.Value == false || 
                                                                     ((r.IsEnglishAndMathsAchieved == null || r.IsEnglishAndMathsAchieved.Value == false) &&
                                                                     (r.IsRcFeed == null || r.IsRcFeed.Value == false))).ToListAsync();

            if (registrationLearners == null) return null;

            return _mapper.Map<IList<RegistrationLearnerDetails>>(registrationLearners);
        }
    }
}
