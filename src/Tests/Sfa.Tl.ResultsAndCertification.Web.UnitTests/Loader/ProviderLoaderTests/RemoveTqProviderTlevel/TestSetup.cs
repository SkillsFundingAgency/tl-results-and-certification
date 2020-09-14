using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.RemoveTqProviderTlevel
{
    public class TestSetup : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int TqProviderId = 1;

        protected bool ApiClientResponse;
        protected bool ActualResult;

        public override void Setup()
        {
            ApiClientResponse = true;
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId)
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
            ActualResult = await Loader.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId);
        }
    }
}
