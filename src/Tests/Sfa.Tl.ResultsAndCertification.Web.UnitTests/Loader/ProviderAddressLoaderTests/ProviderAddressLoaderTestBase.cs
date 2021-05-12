using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests
{
    public abstract class ProviderAddressLoaderTestBase : BaseTest<ProviderAddressLoader>
    {
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        // Dependencies
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IOrdnanceSurveyApiClient OrdnanceSurveyApiClient;
        protected IMapper Mapper;
        protected ILogger<ProviderAddressLoader> Logger;
        protected IHttpContextAccessor HttpContextAccessor;
        protected ProviderAddressLoader Loader;

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, Givenname),
                    new Claim(ClaimTypes.Surname, Surname),
                    new Claim(ClaimTypes.Email, Email)
                }))
            });

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            OrdnanceSurveyApiClient = Substitute.For<IOrdnanceSurveyApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderAddressMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            Logger = Substitute.For<ILogger<ProviderAddressLoader>>();
            Loader = new ProviderAddressLoader(InternalApiClient, OrdnanceSurveyApiClient, Mapper, Logger);
        }
    }
}
