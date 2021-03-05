using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly ITrainingProviderRepository _trainingProviderRepository;
        private readonly IMapper _mapper;

        public TrainingProviderService(ITrainingProviderRepository trainingProviderRepository, IMapper mapper)
        {
            _trainingProviderRepository = trainingProviderRepository; 
            _mapper = mapper;
        }

        public async Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln)
        {
            return await _trainingProviderRepository.FindProvidersUlnAsync(providerUkprn, uln);
        }
    }
}
