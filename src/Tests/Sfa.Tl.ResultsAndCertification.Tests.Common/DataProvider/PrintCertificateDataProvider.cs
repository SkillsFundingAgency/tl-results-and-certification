using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class PrintCertificateDataProvider
    {
        public static PrintCertificate CreatePrintCertificate(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var printCertificate = new PrintCertificateBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(printCertificate);
            }
            return printCertificate;
        }

        public static PrintCertificate CreatePrintCertificate(ResultsAndCertificationDbContext _dbContext, PrintCertificate printCertificate, bool addToDbContext = true)
        {
            if (printCertificate == null)
            {
                printCertificate = new PrintCertificateBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(printCertificate);
            }
            return printCertificate;
        }

        public static IList<PrintCertificate> CreatePrintCertificate(ResultsAndCertificationDbContext _dbContext, IList<PrintCertificate> printCertificates, bool addToDbContext = true)
        {
            if (addToDbContext && printCertificates != null && printCertificates.Count > 0)
            {
                _dbContext.AddRange(printCertificates);
            }
            return printCertificates;
        }
    }
}
