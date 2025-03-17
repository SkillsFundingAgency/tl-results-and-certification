﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TlevelController : ControllerBase, ITlevelController
    {
        private readonly ITlevelService _tlevelService;
        private readonly IPathwayService _pathwayService;

        public TlevelController(ITlevelService tlevelService,
            IPathwayService pathwayService)
        {
            _tlevelService = tlevelService;
            _pathwayService = pathwayService;
        }

        [HttpGet]
        [Route("GetAllTlevels/{ukprn}")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetAllTlevelsByUkprnAsync(long ukprn)
        {
            return await _tlevelService.GetAllTlevelsByUkprnAsync(ukprn);
        }

        [HttpGet]
        [Route("{ukprn}/GetTlevelsByStatus/{statusId}")]
        public async Task<IEnumerable<AwardingOrganisationPathwayStatus>> GetTlevelsByStatusIdAsync(long ukprn, int statusId)
        {
            return await _tlevelService.GetTlevelsByStatusIdAsync(ukprn, statusId);
        }

        [HttpGet]
        [Route("{ukprn}/TlevelDetails/{pathwayId}")]
        public async Task<TlevelPathwayDetails> GetTlevelDetailsByPathwayIdAsync(long ukprn, int pathwayId)
        {
            return await _pathwayService.GetTlevelDetailsByPathwayIdAsync(ukprn, pathwayId);
        }

        [HttpPut]
        [Route("VerifyTlevel")]
        public async Task<IActionResult> VerifyTlevelAsync(VerifyTlevelDetails model)
        {
            var result = await _tlevelService.VerifyTlevelAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("{aoUkprn}/GetPathwaySpecialisms/{pathwayLarId}")]
        public async Task<PathwaySpecialisms> GetPathwaySpecialismsByPathwayLarIdAsync(long aoUkprn, string pathwayLarId)
        {
            return await _pathwayService.GetPathwaySpecialismsByPathwayLarIdAsync(aoUkprn, pathwayLarId);
        }
    }
}