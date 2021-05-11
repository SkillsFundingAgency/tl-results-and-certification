using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProviderAddress
{
    public class When_SingleOrDefault_Is_Called : BaseTest<Domain.Models.TlProviderAddress>
    {
        private Domain.Models.TlProviderAddress _result;
        private Domain.Models.TlProviderAddress _data;

        public override void Given()
        {
            _data = new TlProviderAddressBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_data.Id);
            _result.DepartmentName.Should().Be(_data.DepartmentName);
            _result.AddressLine1.Should().Be(_data.AddressLine1);
            _result.AddressLine2.Should().Be(_data.AddressLine2);
            _result.Town.Should().Be(_data.Town);
            _result.Postcode.Should().Be(_data.Postcode);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
