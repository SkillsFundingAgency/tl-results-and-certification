using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.ResultSlipGeneratorService
{
    public abstract class TestSetup : BaseTest<IResultSlipsGeneratorService>
    {
        protected ILogger<IResultSlipsGeneratorService> Logger;
        protected IBlobStorageService BlobStorageService;
        protected IResultSlipsGeneratorService Service;
        protected byte[] Response;

        public override void Setup()
        {
            BlobStorageService = Substitute.For<IBlobStorageService>();
            Logger = Substitute.For<ILogger<IResultSlipsGeneratorService>>();
            Service = Substitute.For<IResultSlipsGeneratorService>();

            Response = new byte[0];
        }
        public override Task When()
        {
            return Task.CompletedTask;
        }
    }
}
