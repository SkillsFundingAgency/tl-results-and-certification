using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class TrainingProviderService : ITrainingProviderService
    {
        private readonly IMapper _mapper;

        public async Task<FindUlnResponse> FindProvidersUlnAsync(long providerUkprn, long uln)
        {
            throw new NotImplementedException();
        }
    }
}
