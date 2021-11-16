using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService
{
    public abstract class UcasDataTransferServiceTestBase : BaseTest<Functions.Services.UcasDataTransferService>
    {
        // Depedencies
        protected IUcasDataService UcasDataService;
        protected IBlobStorageService BlobStorageService;
        protected IUcasApiClient UcasApiClient;
        protected ILogger<IUcasDataTransferService> Logger;

        // Actual test instance
        protected Functions.Services.UcasDataTransferService Service;

        // Result
        protected UcasDataTransferResponse ActualResult;

        public override void Setup()
        {
            UcasDataService = Substitute.For<IUcasDataService>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            UcasApiClient = Substitute.For<IUcasApiClient>();
            Logger = Substitute.For<ILogger<IUcasDataTransferService>>();

            Service = new Functions.Services.UcasDataTransferService(UcasDataService, BlobStorageService, UcasApiClient, Logger);
        }
    }
}
