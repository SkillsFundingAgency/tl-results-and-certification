using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetailsAsync
{
    public abstract class When_GetTlevelConfirmationDetailsAsync__Is_Called : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly long Ukprn = 9;

        protected IEnumerable<AwardingOrganisationPathwayStatus> ApiClientResponse;
        protected TlevelConfirmationViewModel ActualResult;
        protected int PathwayId = 11;
        protected int PathwayId2 = 22;

        public override void Setup()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = PathwayId, TlevelTitle = "Tlevel Title1", StatusId = 2 },
                new AwardingOrganisationPathwayStatus { Id = 2, PathwayId = PathwayId2, TlevelTitle = "Tlevel Title2", StatusId = 1 },
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn)
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
            ActualResult = Loader.GetTlevelConfirmationDetailsAsync(Ukprn, PathwayId).Result;
        }
    }
}
