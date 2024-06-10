using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificateTrackingExtractionService
{
    public abstract class TestSetup : BaseTest<Functions.Services.CertificateTrackingExtractionService>
    {
        protected ICertificateRepository Repository;
        protected IBlobStorageService BlobStorageService;
        protected IMapper Mapper;
        protected ILogger<ICertificateTrackingExtractionService> Logger;

        protected Functions.Services.CertificateTrackingExtractionService Service;


        public override void Setup()
        {
            Repository = Substitute.For<ICertificateRepository>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            Mapper = new AutoMapper.Mapper(new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly)));
            Logger = Substitute.For<ILogger<ICertificateTrackingExtractionService>>();

            Service = new Functions.Services.CertificateTrackingExtractionService(Repository, BlobStorageService, Mapper, Logger);
        }
    }
}
