using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetTechSpecFile
{
    public abstract class TestSetup : BaseTest<DocumentLoader>
    {
        protected ILogger<DocumentLoader> Logger;
        protected IDocumentLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected Stream ActualResult;
        protected string FileName = "Techspec.xls";
        protected string FolderName = "Registrations";

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<DocumentLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();

            Loader = new DocumentLoader(Logger, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetTechSpecFileAsync(FolderName, FileName);
        }
    }
}
