using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminProvider;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminProviderLoaderTests
{
    public abstract class AdminProviderLoaderBaseTest : BaseTest<AdminProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminProviderLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Loader = new AdminProviderLoader(ApiClient, CreateMapper());
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "test"),
                    new Claim(ClaimTypes.Surname, "user"),
                    new Claim(ClaimTypes.Email, "test.user@test.com")
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AdminProviderLoader).Assembly);

                c.ConstructServicesUsing(type =>
                {
                    if (type.Equals(typeof(UserNameResolver<AdminEditProviderViewModel, UpdateProviderRequest>)))
                    {
                        return new UserNameResolver<AdminEditProviderViewModel, UpdateProviderRequest>(httpContextAccessor);
                    }
                    else
                    {
                        return null;
                    }
                });
            });

            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}