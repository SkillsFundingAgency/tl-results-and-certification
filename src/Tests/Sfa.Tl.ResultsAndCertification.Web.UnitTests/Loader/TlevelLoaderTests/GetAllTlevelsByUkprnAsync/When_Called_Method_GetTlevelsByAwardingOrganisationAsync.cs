using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
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
        protected IEnumerable<YourTlevelsViewModel> ActualResult;
        protected IEnumerable<YourTlevelsViewModel> ExpectedResult;

        protected string PageTitle = "T Level Summary";
        protected int PathwayId = 1;
        protected int StatusId = 1;
        protected string TlevelTitle = "Route: Pathway";

        public override void Setup()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>();
            ExpectedResult = new List<YourTlevelsViewModel> { new YourTlevelsViewModel { PageTitle = PageTitle, PathwayId = PathwayId, StatusId = StatusId, TlevelTitle = TlevelTitle } };

            Mapper = Substitute.For<IMapper>();
            Mapper.Map<IEnumerable<YourTlevelsViewModel>>(ApiClientResponse).Returns(ExpectedResult);

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
            ActualResult = Loader.GetAllTlevelsByUkprnAsync(Ukprn).Result;
        }
    }
}
