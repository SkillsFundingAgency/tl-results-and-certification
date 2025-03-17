using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests
{
    public abstract class AdminAwardingOrganisationControllerBaseTest : BaseTest<AdminAwardingOrganisationController>
    {
        protected IAdminAwardingOrganisationLoader Loader;
        protected IResultLoader ResultLoader;
        protected IPostResultsServiceLoader PostResultsLoader;
        protected ICacheService CacheService;

        protected string CacheKey;
        protected AdminAwardingOrganisationController Controller;

        protected const string UserEmail = "test@email.com";

        public override void Setup()
        {
            Loader = Substitute.For<IAdminAwardingOrganisationLoader>();
            ResultLoader = Substitute.For<IResultLoader>();
            PostResultsLoader = Substitute.For<IPostResultsServiceLoader>();
            CacheService = Substitute.For<ICacheService>();
            Controller = new AdminAwardingOrganisationController(Loader, ResultLoader, PostResultsLoader, CacheService, Substitute.For<ILogger<AdminAwardingOrganisationController>>());

            var httpContext = new ClaimsIdentityBuilder<AdminAwardingOrganisationController>(Controller)
               .Add(CustomClaimTypes.UserId, "08368cd1-cb2e-4197-8774-fbc6ab09ea64")
               .Add(ClaimTypes.Email, UserEmail)
               .Build()
               .HttpContext;

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            CacheKey = CacheKeyHelper.GetCacheKey(httpContext.User.GetUserId(), CacheConstants.AdminAwardingOrganisationCacheKey);
        }

        public override void Given()
        {
        }

        protected static AwardingOrganisationMetadata[] GetAwardingOrganisations()
            => new AwardingOrganisationMetadata[]
            {
                new()
                {
                    Id = 3,
                    Ukprn = 10009931,
                    DisplayName = "City & Guilds"
                },
                new()
                {
                    Id = 1,
                    Ukprn = 10009696,
                    DisplayName = "NCFE"
                },
                new()
                {
                    Id = 2,
                    Ukprn = 10022490,
                    DisplayName = "Pearson"
                }
            };
    }
}