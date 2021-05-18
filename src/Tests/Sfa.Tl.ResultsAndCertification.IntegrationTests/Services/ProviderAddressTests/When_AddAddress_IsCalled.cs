using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderAddressTests
{
    public class When_AddAddress_IsCalled : ProviderAddressServiceBaseTest
    {
        private bool _actualResult;

        public override void Given()
        {
            SeedTestData();

            // Create Service
            CreateMapper();
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

        public async Task WhenAsync(AddAddressRequest request)
        {
            _actualResult = await ProviderAddressService.AddAddressAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(AddAddressRequest request, bool expectedResult)
        {
            await WhenAsync(request);

            var actualAddress = await DbContext.TlProviderAddress.Where(p => p.TlProvider.UkPrn == request.Ukprn && p.TlProvider.IsActive && p.IsActive).OrderByDescending(p => p.CreatedOn).FirstOrDefaultAsync();

            _actualResult.Should().Be(expectedResult);

            if(expectedResult)
            {
                actualAddress.DepartmentName.Should().Be(request.DepartmentName);
                actualAddress.OrganisationName.Should().Be(request.OrganisationName);
                actualAddress.AddressLine1.Should().Be(request.AddressLine1);
                actualAddress.AddressLine2.Should().Be(request.AddressLine2);
                actualAddress.Town.Should().Be(request.Town);
                actualAddress.Postcode.Should().Be(request.Postcode);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {                    
                    new object[]
                    {
                        new AddAddressRequest { Ukprn = 00000000, DepartmentName = "Test Dept", OrganisationName = "Test Org name", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy", PerformedBy = "Test User" },
                        false
                    },
                    new object[]
                    {
                        new AddAddressRequest { Ukprn = (long)Provider.BarsleyCollege, DepartmentName = "Test Dept", OrganisationName = "Test Org name", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "town", Postcode = "xx1 1yy", PerformedBy = "Test User" },
                        true
                    },
                };
            }
        }
    }
}
