using AutoMapper;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrinting
{
    public abstract class TestSetup : BaseTest<Functions.CertificatePrinting>
    {
        protected IMapper Mapper;
        protected ILoggerFactory LoggerFactory;
        protected ILogger<ICertificatePrintingService> Logger;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected ICertificatePrintingService CertificatePrintingService;
        protected Functions.CertificatePrinting CertificatePrintingFunction;
        protected FunctionContext FunctionContext;

        public override void Setup()
        {
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ICertificatePrintingService>>();
            CertificatePrintingService = Substitute.For<ICertificatePrintingService>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            FunctionContext = Substitute.For<FunctionContext>();
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                CertificatePrintingBatchesCreateStartDate = DateTime.UtcNow
            };

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Program).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
            CertificatePrintingFunction = new Functions.CertificatePrinting(ResultsAndCertificationConfiguration, CommonService, CertificatePrintingService,Logger);
        }
    }
}