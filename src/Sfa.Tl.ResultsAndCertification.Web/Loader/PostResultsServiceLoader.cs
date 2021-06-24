﻿using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class PostResultsServiceLoader : IPostResultsServiceLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public PostResultsServiceLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _internalApiClient.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        public async Task<T> GetPrsLearnerDetailsAsync<T>(long aoUkprn, int profileId, int assessmentSeriesId)
        {
            var prsLearnerDetails = await _internalApiClient.GetPrsLearnerDetailsAsync(aoUkprn, profileId, assessmentSeriesId);
            return _mapper.Map<T>(prsLearnerDetails);
        }
    }
}