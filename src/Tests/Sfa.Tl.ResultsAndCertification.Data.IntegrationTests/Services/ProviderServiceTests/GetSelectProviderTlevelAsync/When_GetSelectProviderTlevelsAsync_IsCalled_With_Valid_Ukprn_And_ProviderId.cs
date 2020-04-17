using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetSelectProviderTlevelAsync
{
    public class When_GetSelectProviderTlevelsAsync_IsCalled_With_Valid_Ukprn_And_ProviderId : ProviderServiceBaseTest
    {
        private ProviderTlevels _result;

        public override void Given()
        {            
            SeedTestData();

            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public override void When()
        {
            _result = ProviderService.GetSelectProviderTlevelsAsync(TlAwardingOrganisation.UkPrn, TlProvider.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var actualResult = _result;
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(TlProvider.Id);
            actualResult.DisplayName.Should().Be(TlProvider.DisplayName);
            actualResult.Ukprn.Should().Be(TlProvider.UkPrn);            
        }

        [Fact]
        public void The_Provider_Tlevels_Is_Not_Null()
        {
            _result.Tlevels.Should().NotBeNull();
        }

        [Fact]
        public void Then_Provider_Tlevels_Count_Is_As_Expected()
        {
            _result.Tlevels.Count().Should().Be(1);
        }

        [Fact]
        public void Then_Provider_Tlevels_Data_As_Expected()
        {
            var actualTlevelResult = _result.Tlevels.FirstOrDefault();
            actualTlevelResult.TqAwardingOrganisationId.Should().Be(TqAwardingOrganisation.Id);
            actualTlevelResult.TlProviderId.Should().Be(TlProvider.Id);
            actualTlevelResult.RouteName.Should().Be(Route.Name);
            actualTlevelResult.PathwayName.Should().Be(Pathway.Name);
        }
    }
}
