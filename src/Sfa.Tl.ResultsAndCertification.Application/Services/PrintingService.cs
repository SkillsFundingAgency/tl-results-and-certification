﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
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
        private readonly IRepository<Batch> _batchRepository;
        private readonly IRepository<PrintBatchItem> _printBatchItemRepository;
        private readonly IPrintingRepository _printingRepository;
        private readonly INotificationService _notificationService;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public PrintingService(
            IMapper mapper,
            IRepository<Batch> batchRepository,
            IRepository<PrintBatchItem> printBatchItemRepository,
            IPrintingRepository printingRepository,
            INotificationService notificationService,
            ResultsAndCertificationConfiguration configuration)
        {
            _mapper = mapper;
            _batchRepository = batchRepository;
            _printBatchItemRepository = printBatchItemRepository;
            _printingRepository = printingRepository;
            _notificationService = notificationService;
            _configuration = configuration;
        }

        public async Task<IList<PrintRequest>> GetPendingPrintRequestsAsync()
        {
            var batches = await _printingRepository.GetPendingPrintRequestAsync();

            if (batches == null) return null;

            return _mapper.Map<IList<PrintRequest>>(batches);
        }

        public async Task<CertificatePrintingResponse> UpdatePrintRequestResponsesAsync(List<PrintRequestResponse> printRequestResponses)
        {
            if (printRequestResponses == null || printRequestResponses.Count == 0)
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };

            var batchIds = printRequestResponses.Select(p => p.BatchNumber < 1 ? p.BatchId : p.BatchNumber);

            var batches = await _batchRepository.GetManyAsync(b => batchIds.Contains(b.Id)).ToListAsync();

            var batchIdsToSendEmail = new List<int>();
            var modifiedBatches = new List<Batch>();

            foreach (var printRequestResponse in printRequestResponses)
            {
                var batchId = printRequestResponse.BatchNumber < 1 ? printRequestResponse.BatchId : printRequestResponse.BatchNumber;
                var batch = batches.FirstOrDefault(b => b.Id == batchId);

                if (batch != null)
                {
                    var batchResponseStatus = EnumExtensions.GetEnumByDisplayName<ResponseStatus>(printRequestResponse.Status);

                    if (batchResponseStatus != ResponseStatus.NotSpecified)
                    {
                        var isResponseStatusError = batchResponseStatus == ResponseStatus.Error;
                        DateTime now = DateTime.UtcNow;

                        batch.Status = isResponseStatusError ? BatchStatus.Error : BatchStatus.Accepted;
                        batch.Errors = isResponseStatusError ? JsonConvert.SerializeObject(printRequestResponse.Errors) : null;
                        batch.RunOn = now;
                        batch.ResponseStatus = batchResponseStatus;
                        batch.ResponseMessage = (printRequestResponse.BatchNumber < 1 && isResponseStatusError) ? JsonConvert.SerializeObject(printRequestResponse.Errors) : null;
                        batch.ModifiedOn = now;
                        batch.ModifiedBy = Constants.FunctionPerformedBy;

                        modifiedBatches.Add(batch);

                        if (isResponseStatusError)
                            batchIdsToSendEmail.Add(batch.Id);
                    }
                }
            }

            if (modifiedBatches.Any())
            {
                var response = await _batchRepository.UpdateManyAsync(modifiedBatches);

                if (response > 0 && batchIdsToSendEmail.Any())
                    await SendEmailAsync(batchIdsToSendEmail, Constants.SubmitCertificatePrintingRequest);

                return new CertificatePrintingResponse { IsSuccess = response > 0, PrintingProcessedCount = printRequestResponses.Count, ModifiedCount = modifiedBatches.Count, SavedCount = response };
            }
            else
            {
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = printRequestResponses.Count, ModifiedCount = 0, SavedCount = 0 };
            }
        }

        public async Task<IList<int>> GetPendingPrintBatchesForBatchSummaryAsync()
        {
            var printBatchIds = await _batchRepository
                .GetManyAsync(p => p.Status == BatchStatus.Accepted && p.PrintingStatus != PrintingStatus.Cancelled && (p.PrintingStatus == null || p.PrintingStatus != PrintingStatus.CollectedByCourier))
                .Select(b => b.Id)
                .ToListAsync();

            return printBatchIds;
        }

        public async Task<CertificatePrintingResponse> UpdateBatchSummaryResponsesAsync(List<BatchSummaryResponse> batchSummaryResponses)
        {
            if (batchSummaryResponses == null || batchSummaryResponses.Count == 0)
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };

            var batchIds = new List<int>();

            batchSummaryResponses.ForEach(bs =>
            {
                batchIds.AddRange(bs.BatchSummary.Select(b => b.BatchNumber));
            });

            var printBatchesFromDb = await _batchRepository.GetManyAsync(b => batchIds.Contains(b.Id)).ToListAsync();

            var batchIdsToSendEmail = new List<int>();
            var modifiedPrintBatches = new List<Batch>();

            foreach (var batchSummaryResponse in batchSummaryResponses)
            {
                foreach (var batchSummary in batchSummaryResponse.BatchSummary)
                {
                    var batchSummaryStatus = EnumExtensions.GetEnumByDisplayName<PrintingStatus>(batchSummary.Status);
                    var printBatch = printBatchesFromDb.FirstOrDefault(b => b.Id == batchSummary.BatchNumber);

                    if (printBatch != null)
                    {
                        if (HasErrorStatus(batchSummary.Status))
                        {
                            printBatch.ResponseStatus = ResponseStatus.Error;
                            printBatch.ResponseMessage = batchSummary.ErrorMessage;
                            printBatch.ModifiedOn = DateTime.UtcNow;
                            printBatch.ModifiedBy = Constants.FunctionPerformedBy;
                            modifiedPrintBatches.Add(printBatch);
                            batchIdsToSendEmail.Add(printBatch.Id);
                        }
                        else if (batchSummaryStatus != PrintingStatus.NotSpecified)
                        {
                            printBatch.PrintingStatus = batchSummaryStatus;
                            printBatch.StatusChangedOn = batchSummary.StatusChangeDate;
                            printBatch.ResponseStatus = ResponseStatus.Success;
                            printBatch.ResponseMessage = null;
                            printBatch.ModifiedOn = DateTime.UtcNow;
                            printBatch.ModifiedBy = Constants.FunctionPerformedBy;

                            modifiedPrintBatches.Add(printBatch);
                        }
                    }
                }
            }

            if (modifiedPrintBatches.Any())
            {
                var response = await _batchRepository.UpdateManyAsync(modifiedPrintBatches);

                if (response > 0 && batchIdsToSendEmail.Any())
                    await SendEmailAsync(batchIdsToSendEmail, Constants.FetchCertificatePrintingBatchSummary);

                return new CertificatePrintingResponse { IsSuccess = response > 0, PrintingProcessedCount = batchSummaryResponses.Count, ModifiedCount = modifiedPrintBatches.Count, SavedCount = response };
            }
            else
            {
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = batchSummaryResponses.Count, ModifiedCount = 0, SavedCount = 0 };
            }
        }

        public async Task<IList<int>> GetPendingItemsForTrackBatchAsync()
        {
            var printBatchIds = await _batchRepository
                .GetManyAsync(p => p.PrintingStatus == PrintingStatus.CollectedByCourier && p.PrintBatchItems.Any(b => b.Status != PrintingBatchItemStatus.Delivered && b.Status != PrintingBatchItemStatus.NotDelivered))
                .Select(p => p.Id)
                .ToListAsync();

            return printBatchIds;
        }

        public async Task<CertificatePrintingResponse> UpdateTrackBatchResponsesAsync(List<TrackBatchResponse> trackBatchResponses)
        {
            if (trackBatchResponses == null || trackBatchResponses.Count == 0)
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = 0, ModifiedCount = 0, SavedCount = 0 };

            var batchIds = new List<int>();

            trackBatchResponses.ForEach(tbr =>
            {
                batchIds.AddRange(tbr.DeliveryNotifications.Select(b => b.BatchNumber));
            });

            var printBatchItemsFromDb = await _printBatchItemRepository.GetManyAsync(b => batchIds.Contains(b.Batch.Id), b => b.Batch, b => b.TlProviderAddress, b => b.TlProviderAddress.TlProvider).ToListAsync();

            var modifiedBatches = new List<Batch>();
            var modifiedPrintBatchItems = new List<PrintBatchItem>();
            var batchIdsToSendEmail = new List<int>();

            foreach (var trackBatchResponse in trackBatchResponses)
            {
                foreach (var deliveryNotification in trackBatchResponse.DeliveryNotifications)
                {
                    if (HasErrorStatus(deliveryNotification.Status))
                    {
                        var batchItem = printBatchItemsFromDb.FirstOrDefault(b => b.Id == deliveryNotification.BatchNumber);

                        if (batchItem != null)
                        {
                            batchItem.Batch.ResponseStatus = ResponseStatus.Error;
                            batchItem.Batch.ResponseMessage = deliveryNotification.ErrorMessage;
                            batchItem.Batch.ModifiedOn = DateTime.UtcNow;
                            batchItem.Batch.ModifiedBy = Constants.FunctionPerformedBy;
                            modifiedPrintBatchItems.Add(batchItem);
                            batchIdsToSendEmail.Add(batchItem.Batch.Id);
                        }
                    }
                    else
                    {
                        var printBatchItems = printBatchItemsFromDb.Where(b => b.BatchId == deliveryNotification.BatchNumber);

                        foreach (var printBatchItem in printBatchItems)
                        {
                            var trackingDetail = deliveryNotification.TrackingDetails.FirstOrDefault(x => x.UKPRN.ToInt() == printBatchItem.TlProviderAddress.TlProvider.UkPrn);

                            if (trackingDetail != null)
                            {
                                var trackingDetailStatus = EnumExtensions.GetEnumByDisplayName<PrintingBatchItemStatus>(trackingDetail.Status);

                                if (trackingDetailStatus != PrintingBatchItemStatus.NotSpecified)
                                {
                                    printBatchItem.Status = trackingDetailStatus;
                                    printBatchItem.StatusChangedOn = trackingDetail.StatusChangeDate;
                                    printBatchItem.Reason = !string.IsNullOrWhiteSpace(trackingDetail.Reason) ? trackingDetail.Reason : null;
                                    printBatchItem.TrackingId = !string.IsNullOrWhiteSpace(trackingDetail.TrackingId) ? trackingDetail.TrackingId : null;
                                    printBatchItem.SignedForBy = !string.IsNullOrWhiteSpace(trackingDetail.SignedForBy) ? trackingDetail.SignedForBy : null;
                                    printBatchItem.ModifiedOn = DateTime.UtcNow;
                                    printBatchItem.ModifiedBy = Constants.FunctionPerformedBy;

                                    if (printBatchItem.Batch.ResponseStatus == ResponseStatus.Error)
                                    {
                                        printBatchItem.Batch.ResponseStatus = ResponseStatus.Success;
                                        printBatchItem.Batch.ResponseMessage = null;
                                        printBatchItem.Batch.ModifiedOn = DateTime.UtcNow;
                                        printBatchItem.Batch.ModifiedBy = Constants.FunctionPerformedBy;
                                    }

                                    modifiedPrintBatchItems.Add(printBatchItem);
                                }
                            }
                        }
                    }
                }
            }

            if (modifiedPrintBatchItems.Any())
            {
                var response = await _printBatchItemRepository.UpdateManyAsync(modifiedPrintBatchItems);

                if (response > 0 && batchIdsToSendEmail.Any())
                    await SendEmailAsync(batchIdsToSendEmail, Constants.FetchCertificatePrintingTrackBatch);

                return new CertificatePrintingResponse { IsSuccess = response > 0, PrintingProcessedCount = trackBatchResponses.Count, ModifiedCount = modifiedPrintBatchItems.Count, SavedCount = response };
            }
            else
            {
                return new CertificatePrintingResponse { IsSuccess = true, PrintingProcessedCount = trackBatchResponses.Count, ModifiedCount = 0, SavedCount = 0 };
            }
        }

        private bool HasErrorStatus(string value)
        {
            var responseStatus = EnumExtensions.GetEnumByDisplayName<ResponseStatus>(value);
            return responseStatus switch
            {
                ResponseStatus.Error => true,
                _ => false,
            };
        }

        private async Task<bool> SendEmailAsync(List<int> batchIds, string jobName)
        {
            var tokens = new Dictionary<string, dynamic>
                {
                    { "job_name", jobName },
                    { "batch_ids", batchIds != null && batchIds.Any() ? string.Join(",", batchIds) : string.Empty },
                    { "sender_name", Constants.FunctionPerformedBy }
                };

            return await _notificationService.SendEmailNotificationAsync(NotificationTemplateName.PrintingJobFailedNotification.ToString(), _configuration.TechnicalInternalNotificationEmailAddress, tokens);
        }
    }
}