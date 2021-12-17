using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
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
        public async Task<DataExportResponse> GetDataExportAsync(long aoUkprn, DataExportType requestType, string requestedBy)
        {
            return await _dataExportLoader.ProcessDataExportAsync(aoUkprn, requestType, requestedBy);
        }
    }
}
