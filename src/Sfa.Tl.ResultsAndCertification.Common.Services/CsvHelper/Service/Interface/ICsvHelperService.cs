using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface
{
    public interface ICsvHelperService<in TImportModel, TResponseModel> where TResponseModel : class, new() where TImportModel : FileBaseModel
    {
        Task<IList<TResponseModel>> ReadAndParseFileAsync(TImportModel importModel);
    }
}
