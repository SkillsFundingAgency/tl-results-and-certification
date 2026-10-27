using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificateTrackingExtractionTests
{
    public abstract class TestSetup : BaseTest<CertificateTrackingExtraction>
    {
        protected ILogger<CertificateTrackingExtraction> Logger = Substitute.For<ILogger<CertificateTrackingExtraction>>();
        protected CertificateTrackingExtraction CertificateTrackingExtraction;

        protected ICertificateTrackingExtractionService Service = Substitute.For<ICertificateTrackingExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        private ResultsAndCertificationConfiguration _configuration = new()
        {
            CertificateTrackingExtractSettings = new CertificateTrackingExtractSettings
            {
                NumberOfDays = 60
            }
        };

        public override void Setup()
        {
            CertificateTrackingExtraction = new CertificateTrackingExtraction(Service, CommonService, _configuration);
        }

        public override Task When()
        {
            return CertificateTrackingExtraction.CertificateTrackingExtractAsync(new TimerInfo(), FunctionContextFactory.Create());
        }
    }
}