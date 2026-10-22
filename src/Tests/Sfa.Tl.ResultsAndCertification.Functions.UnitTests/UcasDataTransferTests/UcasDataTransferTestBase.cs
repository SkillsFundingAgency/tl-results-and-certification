using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Services;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests
{
    public abstract class UcasDataTransferTestBase : BaseTest<UcasDataTransfer>
    {
        protected ILogger<UcasDataTransferService> Logger = Substitute.For<ILogger<UcasDataTransferService>>();
        

        // Dependencies
        protected IUcasDataTransferService UcasDataTransferService;
        protected ICommonService CommonService;

        // Actual function instance
        protected UcasDataTransfer UcasDataTransferFunction;

        protected DateTime Today => new(2022, 8, 19);

        public override void Setup()
        {
            UcasDataTransferService = Substitute.For<IUcasDataTransferService>();
            CommonService = Substitute.For<ICommonService>();

            UcasDataTransferFunction = new UcasDataTransfer(UcasDataTransferService, CommonService, new ResultsAndCertificationConfiguration());
        }

        public void Setup(DateTimeRange dateTimeRange)
        {
            var config = new ResultsAndCertificationConfiguration
            {
                UcasTransferAmendmentsSettings = new UcasTransferAmendmentsSettings
                {
                    ValidDateRanges = new[] { dateTimeRange }
                }
            };

            
            UcasDataTransferService = Substitute.For<IUcasDataTransferService>();
            CommonService = Substitute.For<ICommonService>();
            UcasDataTransferFunction = new UcasDataTransfer(UcasDataTransferService, CommonService, config);
        }
    }
}
