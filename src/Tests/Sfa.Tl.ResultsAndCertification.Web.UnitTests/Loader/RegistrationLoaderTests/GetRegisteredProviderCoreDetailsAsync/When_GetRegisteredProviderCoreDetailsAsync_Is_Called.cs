using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredProviderCoreDetailsAsync
{
    public abstract class When_GetRegisteredProviderCoreDetailsAsync_Is_Called : BaseTest<RegistrationLoader>
    {
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderUkprn = 987654321;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected RegistrationLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected IList<PathwayDetails> ApiClientResponse;
        protected SelectCoreViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new List<PathwayDetails>
            {
                new PathwayDetails
                {
                    Id = 1,
                    Name = "Test",
                    Code = "10000111"
                },
                new PathwayDetails
                {
                    Id = 2,
                    Name = "Display",
                    Code = "10000112"
                }
            };

            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetRegisteredProviderCoreDetailsAsync(Ukprn, ProviderUkprn).Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public override void When()
        {
            ActualResult = Loader.GetRegisteredProviderCoreDetailsAsync(Ukprn, ProviderUkprn).Result;
        }
    }
}
