using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class CsvResponseModel<TModel>
    {
        public CsvResponseModel()
        {
            Rows = new List<TModel>();
        }

        public bool IsDirty { get; set; }
        public string ErrorMessage { get; set; }

        public IList<TModel> Rows { get; set; }
    }
}
