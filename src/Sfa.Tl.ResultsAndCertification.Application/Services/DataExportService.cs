using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DataExportService : IDataExportService
    {
        public readonly IDataExportRepository _dataExportRepository;

        public DataExportService(IDataExportRepository dataExportRepository)
        {
            _dataExportRepository = dataExportRepository;
        }

        public async Task<IList<RegistrationsExport>> GetDataExportRegistrationsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportRegistrationsAsync(aoUkprn);
        }

        public async Task<IList<CoreAssessmentsExport>> GetDataExportCoreAssessmentsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportCoreAssessmentsAsync(aoUkprn);
        }

        public async Task<IList<SpecialismAssessmentsExport>> GetDataExportSpecialismAssessmentsAsync(long aoUkprn)
        {
            return await _dataExportRepository.GetDataExportSpecialismAssessmentsAsync(aoUkprn);
        }
    }
}
