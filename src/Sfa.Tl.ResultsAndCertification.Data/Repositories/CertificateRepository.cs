using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Certificates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ILogger<CertificateRepository> _logger;
        private readonly ResultsAndCertificationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public CertificateRepository(ILogger<CertificateRepository> logger, ResultsAndCertificationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
                
        public async Task<CertificateDataResponse> SaveCertificatesPrintingDataAsync(Batch batch, List<OverallResult> overallResults)
        {
            var response = new CertificateDataResponse { IsSuccess = true };

            if (batch != null)
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await _dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        await _dbContext.Batch.AddAsync(batch);
                        var batchCount = await _dbContext.SaveChangesAsync();

                        transaction.Commit();

                        // After successfull transacation populate stats
                        response.BatchId = batch.Id;
                        response.TotalBatchRecordsCreated = batchCount;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex.InnerException);
                        transaction.Rollback();
                        response.IsSuccess = false;
                        // TODO: add message
                    }
                });
            }
            return response;
        }
    }
}
