using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class CsvResponseModel<TModel> : CsvResponseBaseModel where TModel : class, new()
    {
        public CsvResponseModel()
        {
            Rows = new List<TModel>();
        }

        public IList<TModel> Rows { get; set; }
    }
}
