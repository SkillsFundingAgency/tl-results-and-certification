using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetConfirmedTlevels
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly long Ukprn = 1024;
        protected int PathwayId = 1;
        protected int StatusId = 1;

        protected IEnumerable<AwardingOrganisationPathwayStatus> ApiClientResponse;
        protected ConfirmedTlevelsViewModel ActualResult;
        protected AwardingOrganisationPathwayStatus ExpectedResult;
        protected string ExpectedTLevelTitle = "Tlevel Title";

        public override void Setup()
        {
            ExpectedResult = new AwardingOrganisationPathwayStatus { PathwayId = PathwayId, TlevelTitle = "Tlevel Title", StatusId = 1 };
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus> { ExpectedResult };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTlevelsByStatusIdAsync(Ukprn, (int)TlevelReviewStatus.Confirmed)
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
            ActualResult = await Loader.GetConfirmedTlevelsViewModelAsync(Ukprn);
        }
    }
}
