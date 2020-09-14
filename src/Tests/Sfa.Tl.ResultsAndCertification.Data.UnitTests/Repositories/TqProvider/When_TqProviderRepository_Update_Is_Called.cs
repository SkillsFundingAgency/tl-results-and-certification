using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqProvider
{
    public class When_TqProviderRepository_Update_Is_Called : BaseTest<Domain.Models.TqProvider>
    {
        private Domain.Models.TqProvider _result;
        private Domain.Models.TqProvider _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqProviderBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TqAwardingOrganisationId = 2;
            _data.TlProviderId = 2;
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
            _result.Id.Should().Be(1);
            _result.TqAwardingOrganisationId.Should().Be(_data.TqAwardingOrganisationId);
            _result.TlProviderId.Should().Be(_data.TlProviderId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
