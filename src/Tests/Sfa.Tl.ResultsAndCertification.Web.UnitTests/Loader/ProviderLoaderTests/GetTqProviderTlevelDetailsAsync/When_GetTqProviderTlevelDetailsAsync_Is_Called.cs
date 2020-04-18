using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqProviderTlevelDetailsAsync
{
    public class When_GetTqProviderTlevelDetailsAsync_Is_Called : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int TqProviderId = 1;

        protected ProviderTlevelDetails ApiClientResponse;
        protected ProviderTlevelDetailsViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new ProviderTlevelDetails
            {
                Id = 1,
                DisplayName = "Test",
                Ukprn = 10000113,
                ProviderTlevel = new ProviderTlevel
                {
                    TlevelTitle = "Tlevel Title"
                }
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId).Result;
        }
    }
}
