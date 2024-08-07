﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.RemoveProviderTlevelPost
{
    public abstract class TestSetup : BaseTest<ProviderController>
    {
        protected long Ukprn;
        protected int TqProviderId;
        protected int TlProviderId;
        protected IProviderLoader ProviderLoader;
        protected ICacheService CacheService;
        protected ProviderController Controller;
        protected IActionResult Result;
        protected ILogger<ProviderController> Logger;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ProviderTlevelDetailsViewModel ViewModel;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<ProviderController>>();
            Controller = new ProviderController(ProviderLoader, CacheService, Logger);

            // Default value
            ViewModel = new ProviderTlevelDetailsViewModel { Id = TqProviderId, TlProviderId = TlProviderId };

            var httpContext = new ClaimsIdentityBuilder<ProviderController>(Controller)
               .Add(CustomClaimTypes.Ukprn, Ukprn.ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor.HttpContext.Returns(httpContext);
        }

        public async override Task When()
        {
            Result = await Controller.RemoveProviderTlevelAsync(ViewModel);
        }
    }
}
