using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync()
        {
            var allData = await _overallResultRepository.GetManyAsync().ToListAsync();
            var resultsForPrinting = await _overallResultRepository.GetManyAsync(x =>
                                                x.PrintAvailableFrom.HasValue &&
                                                DateTime.Today >= x.PrintAvailableFrom.Value &&
                                                (x.CalculationStatus == CalculationStatus.Completed || x.CalculationStatus == CalculationStatus.PartiallyCompleted) &&
                                                x.CertificateStatus == CertificateStatus.AwaitingProcessing &&
                                                x.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses.Any() &&
                                                x.IsOptedin && x.EndDate == null &&
                                                x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                                x.TqRegistrationPathway.EndDate == null, 
                                                incl => incl.TqRegistrationPathway.TqRegistrationProfile,
                                                incl => incl.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses.OrderByDescending(o => o.CreatedOn).Take(1))
                                            .GroupBy(x => x.TqRegistrationPathway.TqProvider.TlProviderId)
                                            .Select (x => new LearnerResultsPrintingData  { TlProvider = x.First().TqRegistrationPathway.TqProvider.TlProvider, OverallResults = x.ToList() })
                                            .ToListAsync();

            return resultsForPrinting;
        }
    }
}
