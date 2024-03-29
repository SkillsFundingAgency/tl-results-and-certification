﻿using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrinting
{
    public abstract class TestSetup : BaseTest<Functions.CertificatePrinting>
    {
        protected IMapper Mapper;
        protected ILogger<ICertificatePrintingService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected ICertificatePrintingService CertificatePrintingService;
        protected Functions.CertificatePrinting CertificatePrintingFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ICertificatePrintingService>>();
            CertificatePrintingService = Substitute.For<ICertificatePrintingService>();
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                CertificatePrintingBatchesCreateStartDate = DateTime.UtcNow
            };

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            CertificatePrintingFunction = new Functions.CertificatePrinting(ResultsAndCertificationConfiguration, CommonService, CertificatePrintingService);
        }
    }
}
