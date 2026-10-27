using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressExtractTests
{
    public abstract class TestSetup : BaseTest<ProviderAddressExtraction>
    {
        protected ILogger<ProviderAddressExtraction> Logger;

        protected ProviderAddressExtraction ProviderAddressExtraction;

        protected IProviderAddressExtractionService ProviderAddressExtractionService = Substitute.For<IProviderAddressExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        protected int[] AcademicYearsToProcess = new[] { 2020, 2021 };

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ProviderAddressExtraction>>();
            
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                ProviderAddressExtractSettings = new ProviderAddressExtractSettings
                {
                    ProviderAddressAcademicYearsToProcess = AcademicYearsToProcess,
                    ProviderAddressValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(-1),
                            To = today.AddDays(1)
                        }
                    }
                }
            };

            ProviderAddressExtraction = new ProviderAddressExtraction(config, ProviderAddressExtractionService, CommonService);
        }

        public override Task When()
        {
            return ProviderAddressExtraction.ProviderAddressExtractAsync(new TimerInfo(), FunctionContextFactory.Create());
        }
    }
}
