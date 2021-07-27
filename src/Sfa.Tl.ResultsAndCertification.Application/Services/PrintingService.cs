using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PrintingService : IPrintingService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRepository<Batch> _batchRepository;
        private readonly IRepository<PrintCertificate> _printCertificateRepository;
        private readonly IPrintingRepository _printingRepository;

        public PrintingService(IMapper mapper, ILogger<ILearnerRecordService> logger, IRepository<Batch> batchRepository, IRepository<PrintCertificate> printCertificateRepository, IPrintingRepository printingRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _batchRepository = batchRepository;
            _printCertificateRepository = printCertificateRepository;
            _printingRepository = printingRepository;
        }

        public async Task<IList<PrintRequest>> GetPendingPrintRequestsAsync()
        {
            var batches = await _printingRepository.GetPendingPrintRequestAsync();
            
            if (batches == null) return null;

            return _mapper.Map<IList<PrintRequest>>(batches);
        }

        public async Task<CertificatePrintingResponse> UpdatePrintReqeustResponsesAsync(List<PrintRequestResponse> printRequestResponses)
        {
            var batchIds = printRequestResponses.Select(p => p.BatchNumber);

            var batches = await _batchRepository.GetManyAsync(b => batchIds.Contains(b.Id)).ToListAsync();

            var modifiedBatches = new List<Batch>();

            foreach (var printRequestResponse in printRequestResponses)
            {
                var batch = batches.FirstOrDefault(b => b.Id == printRequestResponse.BatchNumber);

                if(batch != null)
                {
                    var batchStatus = GetBatchStatus(printRequestResponse.Status);

                    if (batchStatus != null)
                    {
                        batch.Status = batchStatus.Value;
                        batch.Errors = batchStatus.Value == BatchStatus.Error ? JsonConvert.SerializeObject(printRequestResponse.Errors) : null;
                        batch.ModifiedOn = DateTime.UtcNow;
                        batch.ModifiedBy = "System";

                        modifiedBatches.Add(batch);
                    }
                }
            }

            if (modifiedBatches.Any())
            {
                var response = await _batchRepository.UpdateManyAsync(modifiedBatches);
                return new CertificatePrintingResponse { IsSuccess = response > 0, PrintingProcessedCount = printRequestResponses.Count, ModifiedCount = modifiedBatches.Count, SavedCount = response };
            }
            else
            {
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = printRequestResponses.Count, ModifiedCount = 0, SavedCount = 0 };
            }            
        }

        

        private BatchStatus? GetBatchStatus(string value)
        {
            return value?.ToLower() switch
            {
                "success" => BatchStatus.Accepted,
                "error" => BatchStatus.Error,
                _ => null,
            };
        }
    }
}
