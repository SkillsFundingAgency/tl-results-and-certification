using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository<OverallResult> _overallResultRepository;

        public CertificateService(ResultsAndCertificationConfiguration configuration, IRepository<OverallResult> overallResultRepository)
        {
            _configuration = configuration;
            _overallResultRepository = overallResultRepository;
        }

        public async Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync()
        {
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

        public async Task<List<CertificateResponse>> ProcessCertificatesForPrintingAsync()
        {
            var response = new List<CertificateResponse>();
            var learnerResultsForPrinting = await GetLearnerResultsForPrintingAsync();

            if (learnerResultsForPrinting == null || !learnerResultsForPrinting.Any())
                return null;

            var batchSize = _configuration.CertificatePrintingBatchSettings.ProvidersBatchSize <= 0 ? Constants.CertificatePrintingDefaultProvidersBatchSize : _configuration.CertificatePrintingBatchSettings.ProvidersBatchSize;
            var batchesToProcess = (int)Math.Ceiling(learnerResultsForPrinting.Count / (decimal)batchSize);

            for (var batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                var leanersToProcess = learnerResultsForPrinting.Skip(batchIndex * batchSize).Take(batchSize);
                response.Add(await PreparePrintingBatchesAsync(leanersToProcess));
            }

            return response;
        }

        public async Task<CertificateResponse> PreparePrintingBatchesAsync(IEnumerable<LearnerResultsPrintingData> learnersPrintingData)
        {
            await Task.CompletedTask;
            return new CertificateResponse { IsSuccess = true, TotalRecords = 10, UpdatedRecords = 10, SavedRecords = 10 };
        }
    }
}
