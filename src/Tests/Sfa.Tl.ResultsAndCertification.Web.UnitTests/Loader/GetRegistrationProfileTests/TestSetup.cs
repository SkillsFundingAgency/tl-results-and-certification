using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProfileId = 1;

        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        public IBlobStorageService BlobStorageService { get; private set; }

        protected RegistrationLoader Loader;
        protected ChangeLearnersNameViewModel ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.GetRegistrationProfileAsync<ChangeLearnersNameViewModel>(AoUkprn, ProfileId).Result;
        }
    }
}
