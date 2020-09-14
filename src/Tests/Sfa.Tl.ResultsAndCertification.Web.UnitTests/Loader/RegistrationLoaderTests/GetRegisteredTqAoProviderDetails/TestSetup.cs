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
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredTqAoProviderDetails
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderId = 1;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected RegistrationLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected IList<ProviderDetails> ApiClientResponse;
        protected SelectProviderViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    Id = 1,
                    DisplayName = "Test",
                    Ukprn = 10000111
                },
                new ProviderDetails
                {
                    Id = 2,
                    DisplayName = "Display",
                    Ukprn = 10000112
                }
            };

            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetRegisteredTqAoProviderDetailsAsync(Ukprn);
        }
    }
}
