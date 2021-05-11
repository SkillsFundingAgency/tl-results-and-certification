using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProviderAddress
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TlProviderAddress>
    {
        private Domain.Models.TlProviderAddress _result;
        private IEnumerable<Domain.Models.TlProviderAddress> _data;

        public override void Given()
        {
            _data = new TlProviderAddressBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault(x => x.Id == 1);

            _result.Should().NotBeNull();
            _result.Id.Should().Be(expectedResult.Id);
            _result.DepartmentName.Should().Be(expectedResult.DepartmentName);
            _result.AddressLine1.Should().Be(expectedResult.AddressLine1);
            _result.AddressLine2.Should().Be(expectedResult.AddressLine2);
            _result.Town.Should().Be(expectedResult.Town);
            _result.Postcode.Should().Be(expectedResult.Postcode);
            _result.IsActive.Should().Be(expectedResult.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
