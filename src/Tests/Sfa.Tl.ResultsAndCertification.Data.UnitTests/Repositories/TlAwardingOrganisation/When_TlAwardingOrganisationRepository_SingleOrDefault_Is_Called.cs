using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlAwardingOrganisation
{
    public class When_TlAwardingOrganisationRepository_SingleOrDefault_Is_Called : BaseTest<Domain.Models.TlAwardingOrganisation>
    {
        private Domain.Models.TlAwardingOrganisation _result;
        private Domain.Models.TlAwardingOrganisation _data;

        public override void Given()
        {
            _data = new TlAwardingOrganisationBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetSingleOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_data.Id);
            _result.Name.Should().Be(_data.Name);
            _result.DisplayName.Should().Be(_data.DisplayName);
            _result.UkPrn.Should().Be(_data.UkPrn);

            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
