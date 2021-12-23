using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IDataExportService
    {
        Task<IList<RegistrationsExport>> GetDataExportRegistrationsAsync(long aoUkprn);
        Task<IList<CoreAssessmentsExport>> GetDataExportCoreAssessmentsAsync(long aoUkprn);
        Task<IList<SpecialismAssessmentsExport>> GetDataExportSpecialismAssessmentsAsync(long aoUkprn);
    }
}