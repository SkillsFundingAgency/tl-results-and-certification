using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadIndustryPlacementsFileGet
{
    public abstract class TestSetup : BaseTest<IndustryPlacementImportController>
    {
        protected int? RequestErrorTypeId;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected ICacheService CacheService;
        protected ILogger<IndustryPlacementImportController> Logger;
        protected IndustryPlacementImportController Controller;
        protected UploadIndustryPlacementsRequestViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public override void Setup()
        {
            IndustryPlacementLoader = Substitute.For<IIndustryPlacementLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<IndustryPlacementImportController>>();
            Controller = new IndustryPlacementImportController(IndustryPlacementLoader, CacheService, Logger);
            ViewModel = new UploadIndustryPlacementsRequestViewModel();
        }

        public override Task When()
        {
            Result = Controller.UploadIndustryPlacementsFile(RequestErrorTypeId);
            return Task.CompletedTask;
        }
    }
}
