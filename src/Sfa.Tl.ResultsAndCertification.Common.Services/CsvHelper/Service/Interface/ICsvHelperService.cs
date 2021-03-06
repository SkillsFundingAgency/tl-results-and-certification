﻿using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service.Interface
{
    public interface ICsvHelperService<in TImportModel, TResponseModel, TModel> where TModel : class, new() where TResponseModel : CsvResponseBaseModel, new() where TImportModel : FileBaseModel
    {
        Task<TResponseModel> ReadAndParseFileAsync(TImportModel importModel);
        Task<byte[]> WriteFileAsync<T>(IList<T> data);
    }
}
