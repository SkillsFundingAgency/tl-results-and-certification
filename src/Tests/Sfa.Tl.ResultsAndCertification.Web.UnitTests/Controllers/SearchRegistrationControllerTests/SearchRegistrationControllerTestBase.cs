using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.SearchRegistrationControllerTests
{
    public abstract class SearchRegistrationControllerTestBase : BaseTest<SearchRegistrationController>
    {
        // Dependencies
        protected ISearchRegistrationLoader SearchRegistrationLoader;
        protected IProviderLoader ProviderLoader;
        protected ICacheService CacheService;
        protected ILogger<SearchRegistrationController> Logger;

        protected SearchRegistrationController Controller;

        // HttpContext
        protected const int NcfeUkprn = 10009696;
        protected Guid UserId;
        protected IHttpContextAccessor HttpContextAccessor;
        protected string CacheKey;

        public override void Setup()
        {
            SearchRegistrationLoader = Substitute.For<ISearchRegistrationLoader>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            CacheService = Substitute.For<ICacheService>();
            Logger = Substitute.For<ILogger<SearchRegistrationController>>();
            Controller = new SearchRegistrationController(SearchRegistrationLoader, ProviderLoader, CacheService, Logger);

            var httpContext = new ClaimsIdentityBuilder<SearchRegistrationController>(Controller)
               .Add(CustomClaimTypes.Ukprn, NcfeUkprn.ToString())
               .Add(CustomClaimTypes.UserId, Guid.NewGuid().ToString())
               .Build()
               .HttpContext;

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.SearchRegistrationCacheKey);
        }

        public override void Given()
        {
        }

        protected static FilterLookupData CreateFilter(int id, string name, bool isSelected = false)
            => new()
            {
                Id = id,
                Name = name,
                IsSelected = isSelected
            };
    }
}