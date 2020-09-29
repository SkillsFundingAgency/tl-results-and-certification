using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsToReviewByUkprn
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly long Ukprn = 1024;

        protected IEnumerable<AwardingOrganisationPathwayStatus> ApiClientResponse;
        protected SelectToReviewPageViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = 11, TlevelTitle = "Tlevel Title11", StatusId = 2 },
                new AwardingOrganisationPathwayStatus { Id = 2, PathwayId = 22, TlevelTitle = "Tlevel Title22", StatusId = 2 },
                new AwardingOrganisationPathwayStatus { Id = 3, PathwayId = 33, TlevelTitle = "Tlevel Title33", StatusId = 1 },
                new AwardingOrganisationPathwayStatus { Id = 4, PathwayId = 33, TlevelTitle = "Tlevel Title44", StatusId = 1 }
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

        public async override Task When()
        {
            ActualResult = await Loader.GetTlevelsToReviewByUkprnAsync(Ukprn);
        }
    }
}
