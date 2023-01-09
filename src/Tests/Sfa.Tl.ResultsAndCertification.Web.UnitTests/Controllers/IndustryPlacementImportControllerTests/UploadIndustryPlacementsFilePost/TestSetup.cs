using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadIndustryPlacementsFilePost
{
    public abstract class TestSetup : BaseTest<IndustryPlacementImportController>
    {
        protected long Ukprn;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected ICacheService CacheService;
        protected ILogger<IndustryPlacementImportController> Logger;
        protected IndustryPlacementImportController Controller;
        protected UploadIndustryPlacementsRequestViewModel ViewModel;
        protected UploadIndustryPlacementsResponseViewModel ResponseViewModel;
        protected IFormFile FormFile;
        protected IHttpContextAccessor HttpContextAccessor;
        public IActionResult Result { get; private set; }
        protected Guid BlobUniqueReference;

        public override void Setup()
        {
            Ukprn = 12345;
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            IndustryPlacementLoader = Substitute.For<IIndustryPlacementLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<IndustryPlacementImportController>>();
            Controller = new IndustryPlacementImportController(IndustryPlacementLoader, CacheService, Logger);
            ViewModel = new UploadIndustryPlacementsRequestViewModel();

            var httpContext = new ClaimsIdentityBuilder<IndustryPlacementImportController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.UploadIndustryPlacementsFileAsync(ViewModel);
        }
    }
}
