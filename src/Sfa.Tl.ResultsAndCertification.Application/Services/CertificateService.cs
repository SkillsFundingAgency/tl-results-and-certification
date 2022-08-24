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
            await Task.CompletedTask;
            var resultsForPrinting = await _overallResultRepository.GetManyAsync(x =>
                                                        x.PrintAvailableFrom.HasValue &&
                                                        x.PrintAvailableFrom.Value < DateTime.Today &&
                                                        (x.CalculationStatus == CalculationStatus.Completed ||
                                                         x.CalculationStatus == CalculationStatus.CompletedRommRaised ||
                                                         x.CalculationStatus == CalculationStatus.CompletedAppealRaised)
                                                        // TODO: IsEligibleCertificateStatus,
                                                        // TODO: OptedIn and EndDate is valid to checck in the printing
                                                        ,
                                                        incl => incl.TqRegistrationPathway.TqRegistrationProfile,
                                                        incl => incl.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses)
                                                        .ToListAsync();
            return resultsForPrinting;
        }
    }
}
