using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProvider
{
    public class When_TlProviderRepository_Update_Is_Called : BaseTest<Domain.Models.TlProvider>
    {
        private Domain.Models.TlProvider _result;
        private Domain.Models.TlProvider _data;
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlProviderBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
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
            _result.Name.Should().Be(_data.Name);
            _result.DisplayName.Should().Be(_data.DisplayName);
            _result.UkPrn.Should().Be(_data.UkPrn);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
