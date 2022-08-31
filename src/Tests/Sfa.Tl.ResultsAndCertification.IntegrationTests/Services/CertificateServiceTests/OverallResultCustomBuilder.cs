using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests
{
    public class OverallResultCustomBuilder
    {
        private readonly OverallResult _overallResult;

        public OverallResultCustomBuilder()
        {
            _overallResult = new OverallResultBuilder().Build();
        }

        public OverallResultCustomBuilder(OverallResult overallResult)
        {
            _overallResult = overallResult;
        }

        public OverallResultCustomBuilder WithTqRegistrationPathwayId(int registrationPathwayId)
        {
            _overallResult.TqRegistrationPathwayId = registrationPathwayId;
            return this;
        }

        public OverallResultCustomBuilder WithPrintAvailableFrom(DateTime printAvailableFrom)
        {
            _overallResult.PrintAvailableFrom = printAvailableFrom;
            return this;
        }

        public OverallResultCustomBuilder WithCalculationStatus(CalculationStatus calculationStatus)
        {
            _overallResult.CalculationStatus = calculationStatus;
            return this;
        }

        public OverallResultCustomBuilder WithCertificateStatus(CertificateStatus certificateStatus)
        {
            _overallResult.CertificateStatus = certificateStatus;
            return this;
        }

        public OverallResult Build()
        {
            return _overallResult;
        }

        public OverallResult Save(ResultsAndCertificationDbContext dbContext)
        {
            OverallResultDataProvider.CreateOverallResult(dbContext, new List<OverallResult> { _overallResult });
            dbContext.SaveChanges();

            return _overallResult;
        }
    }
}
