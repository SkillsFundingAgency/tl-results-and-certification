using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IRepository<OverallResult> _overallResultRepository;

        public CertificateService(IRepository<OverallResult> overallResultRepository)
        {
            _overallResultRepository = overallResultRepository;
        }


        public async Task<IList<OverallResult>> GetLearnerResultsForPrintingAsync()
        {
            var resultsForPrinting = await _overallResultRepository.GetManyAsync(x =>
                                                        x.PrintAvailableFrom.HasValue &&
                                                        DateTime.Today >= x.PrintAvailableFrom.Value &&
                                                        (x.CalculationStatus == CalculationStatus.Completed || x.CalculationStatus == CalculationStatus.PartiallyCompleted) &&
                                                        x.CertificateStatus == CertificateStatus.AwaitingProcessing &&
                                                        x.IsOptedin && x.EndDate == null,
                                                        incl => incl.TqRegistrationPathway.TqRegistrationProfile,
                                                        incl => incl.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses)
                                                        .ToListAsync();
            return resultsForPrinting;
        }
    }
}
