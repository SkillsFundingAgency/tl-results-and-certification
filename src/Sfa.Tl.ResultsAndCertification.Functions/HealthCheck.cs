using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions
{
    public class HealthCheck
    {
        private readonly DatabaseFacade _database;
        private readonly ILogger _logger;

        public HealthCheck(ResultsAndCertificationDbContext dbContext, ILogger logger)
        {
            _database = dbContext.Database;
            _logger = logger;
        }

        [Function(Constants.HealthCheck)]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Health check endpoint called.");
                await _database.ExecuteSqlRawAsync("SELECT 1");

                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL Server check failed: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}