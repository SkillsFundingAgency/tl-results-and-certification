using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.DeleteRegistration
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected RegistrationLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected int ProfileId;
        protected bool ApiClientResponse;
        protected bool ActualResult;
        protected ILogger<RegistrationLoader> Logger;
        protected IBlobStorageService BlobStorageService;

        public override void Setup()
        {
            ProfileId = 1;
            ApiClientResponse = true;
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.DeleteRegistrationAsync(Ukprn, ProfileId).Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.DeleteRegistrationAsync(Ukprn, ProfileId);
        }
    }
}
