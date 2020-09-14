using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqAoProviderDetails
{
    public class TestSetup : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderId = 1;

        protected IList<ProviderDetails> ApiClientResponse;
        protected IList<ProviderDetailsViewModel> ActualResult;

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

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetTqAoProviderDetailsAsync(Ukprn);
        }
    }
}
