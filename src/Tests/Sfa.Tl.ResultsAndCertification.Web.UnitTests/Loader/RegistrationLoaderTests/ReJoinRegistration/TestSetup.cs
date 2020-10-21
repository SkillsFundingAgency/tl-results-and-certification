using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.RejoinRegistration
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        protected int ProfileId;
        protected long Uln;
        protected bool ApiClientResponse;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected RegistrationLoader Loader;
        protected RejoinRegistrationResponse ActualResult;
        protected RejoinRegistrationViewModel ViewModel;
        protected IHttpContextAccessor HttpContextAccessor;
        public IBlobStorageService BlobStorageService { get; private set; }

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

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
                c.AddMaps(typeof(RegistrationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<RejoinRegistrationViewModel, RejoinRegistrationRequest>(HttpContextAccessor) : null);
            });

            Uln = 123456789;
            ProfileId = 1;
            Mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.RejoinRegistrationAsync(AoUkprn, ViewModel);
        }
    }
}
