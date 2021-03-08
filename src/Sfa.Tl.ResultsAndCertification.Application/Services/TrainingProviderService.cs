using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<bool> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            var latestPathway = await _tqRegistrationPathwayRepository
                                    .GetManyAsync(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln && 
                                        x.TqProvider.TlProvider.UkPrn == providerUkprn,
                                        navigationPropertyPath: new Expression<Func<TqRegistrationPathway, object>>[] 
                                        {
                                            n => n.TqProvider.TlProvider, 
                                            n => n.TqRegistrationProfile
                                        })
                                    .OrderByDescending(o => o.CreatedOn)
                                    .FirstOrDefaultAsync();

            if (latestPathway == null || 
                !(latestPathway.Status == RegistrationPathwayStatus.Active || latestPathway.Status == RegistrationPathwayStatus.Withdrawn))
                return false;
            else
                return true;
        }
    }
}
