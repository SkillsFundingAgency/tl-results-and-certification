using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetProviderLookupData
{
    public abstract class TestSetup : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;

        protected ProviderLoader Loader;
        protected IEnumerable<ProviderLookupData> ActualResult;

        protected readonly string ProviderName = "Test Provider";
        protected readonly bool IsExactMatch = false;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetProviderLookupDataAsync(ProviderName, IsExactMatch);
        }
    }
}
