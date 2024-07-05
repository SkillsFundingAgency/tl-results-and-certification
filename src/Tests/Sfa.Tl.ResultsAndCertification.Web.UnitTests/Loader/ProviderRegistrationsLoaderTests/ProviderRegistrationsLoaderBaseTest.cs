using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderRegistrationsLoaderTests
{
    public abstract class ProviderRegistrationsLoaderBaseTest : BaseTest<AdminChangeLogLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected IBlobStorageService BlobStorageService;

        protected ProviderRegistrationsLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            
            var logger = Substitute.For<ILogger<ProviderRegistrationsLoader>>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderRegistrationsMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new ProviderRegistrationsLoader(ApiClient, BlobStorageService, mapper, logger);
        }
    }
}