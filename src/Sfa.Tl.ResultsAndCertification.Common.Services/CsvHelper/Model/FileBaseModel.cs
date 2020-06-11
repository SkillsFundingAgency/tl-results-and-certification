using CsvHelper.Configuration.Attributes;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class FileBaseModel : ValidationState
    {
        [Ignore]
        public virtual Stream FileStream { get; set; }
    }
}
