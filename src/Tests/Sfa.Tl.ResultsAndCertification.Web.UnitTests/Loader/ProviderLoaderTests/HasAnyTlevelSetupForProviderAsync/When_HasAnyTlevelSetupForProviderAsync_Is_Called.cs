using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.HasAnyTlevelSetupForProviderAsync
{
    public abstract class When_HasAnyTlevelSetupForProviderAsync_Is_Called : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;

        protected ProviderLoader Loader;
        protected bool ActualResult;

        protected readonly long Ukprn = 1234;
        protected readonly int TlProviderId = 1;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.HasAnyTlevelSetupForProviderAsync(Ukprn, TlProviderId).Result;
        }
    }
}
