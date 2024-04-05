using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminChangeLogLoaderTests
{
    public abstract class AdminChangeLogLoaderBaseTest : BaseTest<AdminChangeLogLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected IIndustryPlacementLoader IndustryPlacementLoader;
        protected AdminChangeLogLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminChangeLogMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminChangeLogLoader(ApiClient, IndustryPlacementLoader, mapper);
        }
    }
}