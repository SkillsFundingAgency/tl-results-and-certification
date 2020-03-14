using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetAllTlevelsByUkprnAsync
{
    public abstract class When_Called_Method_GetTlevelsByUkprnAsync : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly long Ukprn = 9;

        protected IEnumerable<AwardingOrganisationPathwayStatus> ApiClientResponse;
        protected YourTlevelsViewModel ActualResult;
        protected YourTlevelsViewModel ExpectedResult;

        public override void Setup()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = 11, PathwayName = "P1", RouteName = "R1", StatusId = 1  },
                new AwardingOrganisationPathwayStatus { Id = 2, PathwayId = 22, PathwayName = "P2", RouteName = "R2", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 3, PathwayId = 33, PathwayName = "P3", RouteName = "R3", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 4, PathwayId = 44, PathwayName = "P4", RouteName = "R4", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 5, PathwayId = 55, PathwayName = "P5", RouteName = "R5", StatusId = 3  }
            };

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn)
                .Returns(ApiClientResponse);
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetYourTlevelsViewModel(Ukprn).Result;
        }
    }
}
