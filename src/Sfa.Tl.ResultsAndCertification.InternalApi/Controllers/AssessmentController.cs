﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentController : ControllerBase, IAssessmentController
    {
        private readonly IBulkAssessmentLoader _bulkAssementProcess;
        protected IAssessmentService _assessmentService;

        public AssessmentController(IBulkAssessmentLoader bulkAssementProcess, IAssessmentService assessmentService)
        {
            _bulkAssementProcess = bulkAssementProcess;
            _assessmentService = assessmentService;
        }

        [HttpPost]
        [Route("ProcessBulkAssessments")]
        public async Task<BulkAssessmentResponse> ProcessBulkAssessmentsAsync(BulkProcessRequest request)
        {
            return await _bulkAssementProcess.ProcessAsync(request);
        }        

        [HttpGet]
        [Route("GetAvailableAssessmentSeries/{aoUkprn}/{profileId}/{componentType}/{componentIds}")]
        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType, string componentIds)
        {
            var componentIdsData = componentIds?.Split(Constants.PipeSeperator)?.Select(s => s.ToInt())?.ToList();
            return await _assessmentService.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, componentType, componentIdsData);
        }

        [HttpPost]
        [Route("AddAssessmentEntry")]
        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            return await _assessmentService.AddAssessmentEntryAsync(request);
        }

        [HttpGet]
        [Route("GetActiveAssessmentEntryDetails/{aoUkprn}/{assessmentId}/{componentType}")]
        public async Task<AssessmentEntryDetails> GetActiveAssessmentEntryDetailsAsync(long aoUkprn, int assessmentId, ComponentType componentType)
        {
            return componentType switch
            {
                ComponentType.Core => await _assessmentService.GetActivePathwayAssessmentEntryDetailsAsync(aoUkprn, assessmentId),
                ComponentType.Specialism => null,
                ComponentType.NotSpecified => null,
                _ => null
            };
        }

        [HttpGet]
        [Route("GetActiveSpecialismAssessmentEntries/{aoUkprn}/{specialismAssessmentIds}")]
        public async Task<IEnumerable<AssessmentEntryDetails>> GetActiveSpecialismAssessmentEntriesAsync(long aoUkprn, string specialismAssessmentIds)
        {
            var assessmentIds = specialismAssessmentIds?.Split(Constants.PipeSeperator)?.Select(s => s.ToInt())?.ToList();
            return await _assessmentService.GetActiveSpecialismAssessmentEntriesAsync(aoUkprn, assessmentIds);
        }

        [HttpPut]
        [Route("RemoveAssessmentEntry")]
        public async Task<bool> RemoveAssessmentEntryAsync(RemoveAssessmentEntryRequest model)
        {
            return model.ComponentType switch
            {
                ComponentType.Core => await _assessmentService.RemovePathwayAssessmentEntryAsync(model),
                ComponentType.Specialism => await _assessmentService.RemoveSpecialismAssessmentEntryAsync(model),
                ComponentType.NotSpecified => false,
                _ => false
            };
        }

        [HttpGet]
        [Route("GetAssessmentSeries")]
        public async Task<IList<AssessmentSeriesDetails>> GetAssessmentSeriesAsync()
        {
            return await _assessmentService.GetAssessmentSeriesAsync();
        }
    }
}