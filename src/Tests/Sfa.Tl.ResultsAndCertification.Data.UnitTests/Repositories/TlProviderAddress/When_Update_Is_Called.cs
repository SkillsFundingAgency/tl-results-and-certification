using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProviderAddress
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.TlProviderAddress>
    {
        private Domain.Models.TlProviderAddress _result;
        private Domain.Models.TlProviderAddress _data;
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlProviderAddressBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.Postcode = "zz1 1bb";
            _data.ModifiedBy = ModifiedBy;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_data.Id);
            _result.DepartmentName.Should().Be(_data.DepartmentName);
            _result.OrganisationName.Should().Be(_data.OrganisationName);
            _result.AddressLine1.Should().Be(_data.AddressLine1);
            _result.AddressLine2.Should().Be(_data.AddressLine2);
            _result.Town.Should().Be(_data.Town);
            _result.Postcode.Should().Be(_data.Postcode);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
