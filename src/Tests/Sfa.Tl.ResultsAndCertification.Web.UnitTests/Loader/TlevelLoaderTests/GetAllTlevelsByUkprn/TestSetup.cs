using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetAllTlevelsByUkprn
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
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
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = 11, TlevelTitle = "P1", StatusId = 1  },
                new AwardingOrganisationPathwayStatus { Id = 2, PathwayId = 22, TlevelTitle = "P2", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 3, PathwayId = 33, TlevelTitle = "P3", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 4, PathwayId = 44, TlevelTitle = "P4", StatusId = 2  },
                new AwardingOrganisationPathwayStatus { Id = 5, PathwayId = 55, TlevelTitle = "P5", StatusId = 3  }
            };

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(TlevelMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn).Returns(ApiClientResponse);
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetYourTlevelsViewModel(Ukprn);
        }
    }
}
