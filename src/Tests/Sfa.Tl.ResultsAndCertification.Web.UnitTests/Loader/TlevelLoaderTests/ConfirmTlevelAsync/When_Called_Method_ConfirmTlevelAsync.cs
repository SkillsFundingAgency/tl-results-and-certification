using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.ConfirmTlevelAsync
{
    public abstract class When_Called_Method_ConfirmTlevelAsync : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected readonly long Ukprn = 9;

        protected bool ActualResult;
        protected bool ExpectedResult;

        protected ConfirmTlevelViewModel ConfirmTlevelViewModel;
        protected VerifyTlevelDetails VerifyTlevelDetails;

        protected string PageTitle = "T Level Summary";
        protected int PathwayId = 1;
        protected int StatusId = 2;
        protected string TlevelTitle = "Route: Pathway";

        public override void Setup()
        {
            Mapper = Substitute.For<IMapper>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
        }

        public override void Given()
        {
            ExpectedResult = true;

            ConfirmTlevelViewModel = new ConfirmTlevelViewModel { PathwayId = PathwayId, TqAwardingOrganisationId = PathwayId };
            VerifyTlevelDetails = new VerifyTlevelDetails { Id = PathwayId, TqAwardingOrganisationId = PathwayId, PathwayStatusId = StatusId };
            
            Mapper.Map<VerifyTlevelDetails>(ConfirmTlevelViewModel).Returns(VerifyTlevelDetails);
            InternalApiClient.VerifyTlevelAsync(VerifyTlevelDetails).Returns(ExpectedResult);

            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.ConfirmTlevelAsync(ConfirmTlevelViewModel).Result;
        }
    }
}
