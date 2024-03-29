﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostResultsServiceController : ControllerBase, IPostResultsServiceController
    {
        protected IPostResultsServiceService _postResultsServiceService;

        public PostResultsServiceController(IPostResultsServiceService postResultsServiceService)
        {
            _postResultsServiceService = postResultsServiceService;
        }

        [HttpGet]
        [Route("FindPrsLearnerRecord/{aoUkprn}/{uln}")]
        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _postResultsServiceService.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }

        [HttpGet]
        [Route("FindPrsLearnerRecordByProfileId/{aoUkprn}/{profileId}")]
        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordByProfileIdAsync(long aoUkprn, int profileId)
        {
            return await _postResultsServiceService.FindPrsLearnerRecordAsync(aoUkprn, null, profileId);
        }

        [HttpPost]
        [Route("PrsActivity")]
        public async Task<bool> PrsActivityAsync(PrsActivityRequest request)
        {
            return await _postResultsServiceService.PrsActivityAsync(request);
        }

        [HttpPost]
        [Route("PrsGradeChangeRequest")]
        public async Task<bool> PrsGradeChangeRequestAsync(PrsGradeChangeRequest request)
        {
            return await _postResultsServiceService.PrsGradeChangeRequestAsync(request);
        }
    }
}