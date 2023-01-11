using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests
{
    public abstract class IndustryPlacementControllerTestBase : BaseTest<IndustryPlacementController>
    {
        // Dependencies
        protected IMapper Mapper;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected ICacheService CacheService;
        protected ILogger<IndustryPlacementController> Logger;
        protected IndustryPlacementController Controller;

        // HttpContext
        protected int ProviderUkprn;

        public HttpContext HttpContext { get; set; }

        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;
        protected string TrainingProviderCacheKey;

        public override void Setup()
        {
            IndustryPlacementLoader = Substitute.For<IIndustryPlacementLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<IndustryPlacementController>>();
            Controller = new IndustryPlacementController(IndustryPlacementLoader, CacheService, Logger);

            ProviderUkprn = 1234567890;
            HttpContext = new ClaimsIdentityBuilder<IndustryPlacementController>(Controller)
               .Add(CustomClaimTypes.Ukprn, ProviderUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(HttpContext);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(IndustryPlacementMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            CacheKey = CacheKeyHelper.GetCacheKey(HttpContext.User.GetUserId(), CacheConstants.IpCacheKey);
            TrainingProviderCacheKey = CacheKeyHelper.GetCacheKey(HttpContext.User.GetUserId(), CacheConstants.TrainingProviderCacheKey);
        }
    }
}
