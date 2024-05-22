using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class CsvStringToListParser
    {
        private const char QuotationMark = '"', Space = ' ', Comma = ',';

        public static IList<string> Parse(string specialismCodes)
            => string.IsNullOrWhiteSpace(specialismCodes) switch
            {
                true => new List<string>(),
                false => specialismCodes
                            .Trim(QuotationMark, Space)
                            .Split(Comma)
                            .Select(s => s.Trim())
                            .ToList()
            };
    }
}