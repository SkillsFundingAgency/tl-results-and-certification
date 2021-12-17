using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IDataExportRepository
    {
        Task<IList<RegistrationsExport>> GetDataExportRegistrationsAsync(long aoUkprn);
    }
}
