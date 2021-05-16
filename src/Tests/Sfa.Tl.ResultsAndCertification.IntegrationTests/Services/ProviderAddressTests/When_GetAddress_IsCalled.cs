using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderAddressTests
{
    public class When_GetAddress_IsCalled : ProviderAddressServiceBaseTest
    {
        private Address _actualResult;
        private TlProviderAddress _newAddress;

        public override void Given()
        {
            SeedTestData();            
            CreateMapper();

            SeedProviderAddress();

            _newAddress = new TlProviderAddress
            {
                TlProviderId = TlProviders.First().Id,
                DepartmentName = "New Dept",
                OrganisationName = "New Org",
                AddressLine1 = "New Line1",
                AddressLine2 = "New Line2",
                Town = "New town",
                Postcode = "A11, 7BB",
                IsActive = true,
                CreatedBy = "Test user",
                CreatedOn = DateTime.UtcNow
            };

            AddProviderAddress(_newAddress);

            TlProviderRepositoryLogger = new Logger<GenericRepository<TlProvider>>(new NullLoggerFactory());
            TlProviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);

            TlProviderAddressLogger = new Logger<GenericRepository<TlProviderAddress>>(new NullLoggerFactory());
            TlProviderAddressRepository = new GenericRepository<TlProviderAddress>(TlProviderAddressLogger, DbContext);

            ProviderAddressServiceLogger = new Logger<ProviderAddressService>(new NullLoggerFactory());

            ProviderAddressService = new ProviderAddressService(TlProviderRepository, TlProviderAddressRepository, ProviderAddressMapper, ProviderAddressServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn)
        {
            _actualResult = await ProviderAddressService.GetAddressAsync(providerUkprn);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, bool expectedResult)
        {
            await WhenAsync(providerUkprn);            

            if (expectedResult)
            {
                _actualResult.Should().NotBeNull();
                _actualResult.DepartmentName.Should().Be(_newAddress.DepartmentName);
                _actualResult.OrganisationName.Should().Be(_newAddress.OrganisationName);
                _actualResult.AddressLine1.Should().Be(_newAddress.AddressLine1);
                _actualResult.AddressLine2.Should().Be(_newAddress.AddressLine2);
                _actualResult.Town.Should().Be(_newAddress.Town);
                _actualResult.Postcode.Should().Be(_newAddress.Postcode);
            }
            else
            {
                _actualResult.Should().BeNull();
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { (long)Provider.WalsallCollege, false },
                    new object[] { (long)Provider.BarsleyCollege, true }
                };
            }
        }
    }
}
