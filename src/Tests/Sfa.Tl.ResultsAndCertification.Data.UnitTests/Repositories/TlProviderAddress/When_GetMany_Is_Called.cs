using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProviderAddress
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.TlProviderAddress>
    {
        private IEnumerable<Domain.Models.TlProviderAddress> _result;
        private IList<Domain.Models.TlProviderAddress> _data;

        public override void Given()
        {
            _data = new TlProviderAddressBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }


        [Fact]
        public void Then_The_Expected_Number_Of_Results_Is_Returned()
        {
            _result.Count().Should().Be(_data.Count);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.DepartmentName.Should().Be(expectedResult.DepartmentName);
            actualResult.OrganisationName.Should().Be(expectedResult.OrganisationName);
            actualResult.AddressLine1.Should().Be(expectedResult.AddressLine1);
            actualResult.AddressLine2.Should().Be(expectedResult.AddressLine2);
            actualResult.Town.Should().Be(expectedResult.Town);
            actualResult.Postcode.Should().Be(expectedResult.Postcode);
            actualResult.IsActive.Should().Be(expectedResult.IsActive);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
