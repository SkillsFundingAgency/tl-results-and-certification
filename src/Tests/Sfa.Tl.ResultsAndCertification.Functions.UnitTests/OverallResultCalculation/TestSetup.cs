using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.OverallResultCalculation
{
    public abstract class TestSetup : BaseTest<Functions.OverallResultCalculation>
    {
        protected ILogger<Functions.OverallResultCalculation> Logger;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IOverallResultCalculationFunctionService OverallResultCalculationFunctionService;
        protected Functions.OverallResultCalculation OverallResultCalculationFunction;
        protected FunctionContext FunctionContext;

        public override void Setup()
        {
            FunctionContext = Substitute.For<FunctionContext>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<Functions.OverallResultCalculation>>();
            OverallResultCalculationFunctionService = Substitute.For<IOverallResultCalculationFunctionService>();
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultsCalculationDate = DateTime.UtcNow
            };
            OverallResultCalculationFunction = new Functions.OverallResultCalculation(ResultsAndCertificationConfiguration, OverallResultCalculationFunctionService, CommonService, Logger);
        }

        public async override Task When()
        {
            await OverallResultCalculationFunction.OverallResultCalculationAsync(new TimerInfo(), FunctionContext);
        }
    }
}
