﻿using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayId
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected TLevelDetailsViewModel ActualResult;
        protected readonly int Id = 9;
        protected readonly long Ukprn = 1024;
        protected TlevelPathwayDetails ApiClientResponse;
        protected TLevelDetailsViewModel ExpectedResult;

        protected readonly int PathwayId = 1;
        protected readonly string PathwayName = "Pathway Name1";
        protected readonly string RouteName = "Route Name1";
        protected readonly bool ShowSomethingIsNotRight = true;
        protected readonly bool ShowQueriedInfo = true;
        protected readonly List<string> Specialisms = new List<string> { "Spl1", "Spl2" };

        public override void Setup()
        {
            ApiClientResponse = new TlevelPathwayDetails { PathwayId = 1, PathwayName = PathwayName, RouteName = RouteName, PathwayStatusId = 2, Specialisms = Specialisms };
            ExpectedResult = new TLevelDetailsViewModel { PathwayId = 1, PathwayName = PathwayName, RouteName = RouteName, ShowSomethingIsNotRight = ShowSomethingIsNotRight, ShowQueriedInfo = ShowQueriedInfo, Specialisms = Specialisms };

            Mapper = Substitute.For<IMapper>();
            Mapper.Map<TLevelDetailsViewModel>(ApiClientResponse).Returns(ExpectedResult);

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id)
                .Returns(ApiClientResponse);
        }

        public override void Given()
        {
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }
    }
}
