using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.ViewProviderTlevels;
using System.Collections.Generic;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetViewProviderTlevelViewModel
{
    public abstract class When_GetViewProviderTlevelViewModelAsync_Is_Called : BaseTest<ProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ProviderLoader Loader;
        protected readonly long Ukprn = 12345678;
        protected readonly int ProviderId = 1;

        protected ProviderTlevels ApiClientResponse;
        protected ProviderViewModel ActualResult;

        public override void Setup()
        {
            ApiClientResponse = new ProviderTlevels
            {
                Id = 1,
                DisplayName = "Test1",
                Ukprn = 12345,
                Tlevels = new List<ProviderTlevel>
                    {
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 7, TlevelTitle = "Tlevel Title1", RouteName = "Route1", PathwayName = "Pathway1", TqProviderId = 10 },
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 7, TlevelTitle = "Tlevel Title2", RouteName = "Route2", PathwayName = "Pathway2", TqProviderId = 11 },
                        new ProviderTlevel { TqAwardingOrganisationId = 1, TlProviderId = 7, TlevelTitle = "Tlevel Title3", RouteName = "Route3", PathwayName = "Pathway3", TqProviderId = 22 }
                    }
            };

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.GetAllProviderTlevelsAsync(Ukprn, ProviderId)
                .Returns(ApiClientResponse);

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        public override void When()
        {
            ActualResult = Loader.GetViewProviderTlevelViewModelAsync(Ukprn, ProviderId).Result;
        }
    }
}
