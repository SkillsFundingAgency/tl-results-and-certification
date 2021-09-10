using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueriedTlevelDetails
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;

        // Params
        protected readonly long Ukprn = 1024;
        protected int PathwayId = 11;

        protected TlevelPathwayDetails ApiClientResponse;
        protected TlevelQueriedDetailsViewModel ActualResult;
        protected AwardingOrganisationPathwayStatus ExpectedResult;

        public override void Setup()
        {
            ExpectedResult = new AwardingOrganisationPathwayStatus { PathwayId = PathwayId, TlevelTitle = "Tlevel Title", StatusId = 1 };
            ApiClientResponse = new TlevelPathwayDetails
            {
                TqAwardingOrganisationId = 1,
                RouteId = 2,
                PathwayId = 3,
                RouteName = "Test Route",
                PathwayName = "Test Pathway",
                PathwayCode = "12345678",
                TlevelTitle = "T Level in Education",
                PathwayStatusId = 3,
                Specialisms = new List<SpecialismDetails> {
                    new SpecialismDetails { Name = "Civil Engineering", Code = "97865897" },
                    new SpecialismDetails { Name = "Assisting teaching", Code = "7654321" }
                },
                VerifiedBy = "Test User",
                VerifiedOn = DateTime.Now
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

        public async override Task When()
        {
            ActualResult = await Loader.GetQueriedTlevelDetailsAsync(Ukprn, PathwayId);
        }
    }
}
