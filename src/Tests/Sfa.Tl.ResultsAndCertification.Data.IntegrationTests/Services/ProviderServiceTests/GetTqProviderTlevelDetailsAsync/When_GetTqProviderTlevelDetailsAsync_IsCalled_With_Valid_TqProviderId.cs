using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetTqProviderTlevelDetailsAsync
{
    public class When_GetTqProviderTlevelDetailsAsync_IsCalled_With_Valid_TqProviderId : ProviderServiceBaseTest
    {
        private ProviderTlevelDetails _result;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public override void When()
        {
            _result = ProviderService.GetTqProviderTlevelDetailsAsync(TlAwardingOrganisation.UkPrn, TqProvider.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(TlProvider.Id);
            _result.DisplayName.Should().Be(TlProvider.DisplayName);
            _result.Ukprn.Should().Be(TlProvider.UkPrn);
            _result.ProviderTlevel.TlevelTitle.Should().Be(Pathway.TlevelTitle);
        }

        protected override void SeedProviderData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
        }
    }
}
