using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueryTlevelViewModel
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        // Dependencies
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;

        // Data Objects
        protected TlevelLoader Loader;
        protected TlevelPathwayDetails ApiClientResponse;
        protected TlevelQueryViewModel ActualResult;

        // Params
        protected readonly long Ukprn = 9;
        protected int PathwayId = 11;

        public override void Setup()
        {
            ApiClientResponse = new TlevelPathwayDetails 
            {
                TqAwardingOrganisationId = 1,
                RouteId = 2,
                PathwayId = 3, 
                RouteName = "Test Route",
                PathwayName = "Test Pathway",
                PathwayStatusId = 1,
                Specialisms = new List<string> { "Spl1", "Spl2" }
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTlevelDetailsByPathwayIdAsync(Ukprn, PathwayId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetQueryTlevelViewModelAsync(Ukprn, PathwayId).Result;
        }
    }
}
