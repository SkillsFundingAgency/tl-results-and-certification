using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests
{
    public abstract class TrainingProviderLoaderTestBase : BaseTest<TrainingProviderLoader>
    {
        // Dependencies
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;

        protected TrainingProviderLoader Loader;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TrainingProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }
    }
}
