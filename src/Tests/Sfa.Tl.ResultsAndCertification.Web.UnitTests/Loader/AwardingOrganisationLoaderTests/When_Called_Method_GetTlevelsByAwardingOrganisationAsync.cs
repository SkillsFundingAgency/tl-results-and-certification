using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AwardingOrganisationLoaderTests
{
    public abstract class When_Called_Method_GetTlevelsByUkprnAsync : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        private IEnumerable<YourTlevelsViewModel> Result;

        public override void Setup()
        {
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Mapper = Substitute.For<IMapper>();

            InternalApiClient.GetAllTlevelsByUkprnAsync(9)
                .Returns(new List<AwardingOrganisationPathwayStatus>());
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            Result = Loader.GetAllTlevelsByUkprnAsync(9).Result;
        }
    }
}
