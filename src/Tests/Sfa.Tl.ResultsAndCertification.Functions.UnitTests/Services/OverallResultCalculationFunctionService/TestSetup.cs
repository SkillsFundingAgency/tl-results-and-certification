using AutoMapper;

using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.OverallResultCalculationFunctionService
{
    public abstract class TestSetup : BaseTest<Functions.Services.LrsLearnerService>
    {
        protected IMapper Mapper;
        protected ILogger<IOverallResultCalculationFunctionService> Logger;
        protected IOverallResultCalculationService OverallResultCalculationService;
        protected Functions.Services.OverallResultCalculationFunctionService Service;
        protected IList<OverallResultResponse> ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<IOverallResultCalculationFunctionService>>();
            OverallResultCalculationService = Substitute.For<IOverallResultCalculationService>();

            Service = new Functions.Services.OverallResultCalculationFunctionService(OverallResultCalculationService, Logger);
        }

        public async override Task When()
        {
            ActualResult = await Service.CalculateOverallResultsAsync();
        }
    }
}
