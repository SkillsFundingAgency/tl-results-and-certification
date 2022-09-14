using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Certificates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class CertificateRepository : GenericRepository<OverallResult>, ICertificateRepository
    {
        private readonly ILogger<CertificateRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public CertificateRepository(ILogger<CertificateRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        /// <summary>
        /// Saves the certificates printing data asynchronous.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="overallResults">The overall results.</param>
        /// <returns><see cref="CertificateDataResponse"/> returns certificate data response</returns>
        public async Task<CertificateDataResponse> SaveCertificatesPrintingDataAsync(Batch batch, List<OverallResult> overallResults)
        {
            var response = new CertificateDataResponse { IsSuccess = true };

            if (batch != null && overallResults != null && overallResults.Count > 0)
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        await _dbContext.Batch.AddAsync(batch);
                        var batchCount = await _dbContext.SaveChangesAsync();

                        // update CertificateStatus from AwaitingProcessing to Processed
                        overallResults.ForEach(x => x.CertificateStatus = CertificateStatus.Processed);

                        // Update overallresult table for specified column (changed only columns)
                        var overallResultsCount = await UpdateWithSpecifedColumnsOnlyAsync(overallResults, x => x.CertificateStatus);
                        
                        transaction.Commit();

                        // After successfull transacation populate stats
                        response.BatchId = batch.Id;
                        response.TotalBatchRecordsCreated = batchCount;
                        response.OverallResultsUpdatedCount = overallResultsCount;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex.InnerException);
                        transaction.Rollback();
                        response.IsSuccess = false;
                        response.Message = $"Exception: {ex}";
                    }
                });
            }
            return response;
        }
    }
}
