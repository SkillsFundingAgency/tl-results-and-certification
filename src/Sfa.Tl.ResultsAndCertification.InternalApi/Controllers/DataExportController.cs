﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataExportController : ControllerBase, IDataExportController
    {
        private readonly IDataExportLoader _dataExportLoader;

        public DataExportController(IDataExportLoader dataExportLoader)
        {
            _dataExportLoader = dataExportLoader;
        }

        [HttpGet]
        [Route("GetDataExport/{aoUkprn}/{requestType}/{requestedBy}")]
        public async Task<IList<DataExportResponse>> GetDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            return await _dataExportLoader.ProcessDataExportAsync(aoUkprn, requestType, requestedBy);
        }

        [HttpGet]
        [Route("DownloadOverallResultsData/{providerUkprn}/{requestedBy}")]
        public async Task<DataExportResponse> DownloadOverallResultsDataAsync(long providerUkprn, string requestedBy)
        {
            return await _dataExportLoader.DownloadOverallResultsDataAsync(providerUkprn, requestedBy);
        }

        [HttpGet]
        [Route("DownloadOverallResultSlipsData/{providerUkprn}/{requestedBy}")]
        public async Task<DataExportResponse> DownloadOverallResultSlipsDataAsync(long providerUkprn, string requestedBy)
        {
            return await _dataExportLoader.DownloadOverallResultSlipsDataAsync(providerUkprn, requestedBy);
        }


        [HttpGet]
        [Route("DownloadLearnerOverallResultSlipsData/{providerUkprn}/{profileId}/{requestedBy}")]
        public async Task<DataExportResponse> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, int profileId, string requestedBy)
        {
            return await _dataExportLoader.DownloadLearnerOverallResultSlipsDataAsync(providerUkprn, profileId, requestedBy);
        }

        [HttpGet]
        [Route("DownloadRommExport/{aoUkprn}/{requestedBy}")]
        public async Task<IList<DataExportResponse>> DownloadRommExportAsync(long aoUkprn, string requestedBy)
           => await _dataExportLoader.DownloadRommExportAsync(aoUkprn, requestedBy);
    }
}
