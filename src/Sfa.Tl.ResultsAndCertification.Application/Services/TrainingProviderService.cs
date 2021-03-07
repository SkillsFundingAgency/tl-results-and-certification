using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IMapper _mapper;

        public TrainingProviderService(IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository, IMapper mapper)
        {
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository; 
            _mapper = mapper;
        }

        public async Task<bool> FindProvidersUlnAsync(long providerUkprn, long uln)
        {
            var latestPathway = await _tqRegistrationPathwayRepository
                                    .GetManyAsync(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln, navigationPropertyPath: n => n.TqProvider.TlProvider)
                                    .OrderByDescending(o => o.CreatedOn)
                                    .FirstOrDefaultAsync();

            if (latestPathway == null)
                return false;

            return latestPathway.TqProvider.TlProvider.UkPrn == providerUkprn;

        }
    }
}
