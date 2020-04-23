using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevelsAsync
{
    public abstract class When_Called_Method_AddProviderTlevelsAsync : BaseTest<ProviderLoader>
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderId = 1;
        protected bool ActualResult;
        protected bool ExpectedResult;
        protected ProviderTlevelsViewModel ProviderTlevelsViewModel;
        protected List<ProviderTlevel> ProviderTlevelDetails;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        public override void Setup()
        {
            ProviderTlevelDetails = new List<ProviderTlevel>
                    {
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 1, CreatedBy = "test user" },
                        new ProviderTlevel { TqAwardingOrganisationId = 2, TlProviderId = 1, CreatedBy = "test user" }
                    };

            ProviderTlevelsViewModel = new ProviderTlevelsViewModel
            {
                ProviderId = 1,
                DisplayName = "Test1",
                Ukprn = 12345,
                Tlevels = new List<ProviderTlevelViewModel>
                    {
                        new ProviderTlevelViewModel { TqAwardingOrganisationId = 1, TlProviderId = 1, IsSelected = true },
                        new ProviderTlevelViewModel { TqAwardingOrganisationId = 2, TlProviderId = 1, IsSelected = true }
                    }
            };

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

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<ProviderTlevelViewModel, ProviderTlevel>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
        }

        public override void Given()
        {
            ExpectedResult = true;            
            InternalApiClient.AddProviderTlevelsAsync(Arg.Any<List<ProviderTlevel>>())
                .Returns(ExpectedResult);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.AddProviderTlevelsAsync(ProviderTlevelsViewModel).Result;
        }
    }
}
