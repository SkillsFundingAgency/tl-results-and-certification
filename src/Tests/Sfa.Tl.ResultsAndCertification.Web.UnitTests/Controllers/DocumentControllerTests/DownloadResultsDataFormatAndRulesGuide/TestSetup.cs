using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadResultsDataFormatAndRulesGuide
{
    public abstract class TestSetup : BaseTest<DocumentController>
    {
        protected IDocumentLoader DocumentLoader;
        protected DocumentController Controller;

        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            DocumentLoader = Substitute.For<IDocumentLoader>();
            Controller = new DocumentController(DocumentLoader, Substitute.For<ILogger<DocumentController>>());
        }

        public async override Task When()
        {
            Result = await Controller.DownloadResultsDataFormatAndRulesGuideAsync();
        }
    }
}