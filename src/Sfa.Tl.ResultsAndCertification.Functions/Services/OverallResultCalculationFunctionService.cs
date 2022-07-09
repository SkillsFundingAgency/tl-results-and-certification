using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class OverallResultCalculationFunctionService : IOverallResultCalculationFunctionService
    {
        private readonly IOverallResultCalculationService _resultCalculationService;
        private readonly ILogger _logger;

        public OverallResultCalculationFunctionService(
            IOverallResultCalculationService resultCalculationService,
            ILogger<IUcasDataTransferService> logger)
        {
            _resultCalculationService = resultCalculationService;
            _logger = logger;
        }

        public async Task<FunctionResponse> CalculateOverallResultsAsync()
        {
            var rundate = System.DateTime.Now;
            var isSuccess = await _resultCalculationService.CalculateOverallResultsAsync(rundate);
            return new FunctionResponse { IsSuccess = true };
        }
    }
}