using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class ResultService : IResultService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMapper _mapper;

        public ResultService(IAssessmentRepository assessmentRepository, IMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }

        public async Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistration = await _assessmentRepository.GetAssessmentsAsync(aoUkprn, profileId);

            if (tqRegistration == null || (status != null && tqRegistration.Status != status)) return null;

            return _mapper.Map<ResultDetails>(tqRegistration);
        }
    }
}
