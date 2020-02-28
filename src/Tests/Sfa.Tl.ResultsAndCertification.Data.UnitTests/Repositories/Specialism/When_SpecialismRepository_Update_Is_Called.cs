using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Specialism
{
    public class When_SpecialismRepository_Update_Is_Called : BaseTest<TlSpecialism>
    {
        private TlSpecialism _result;
        private TlSpecialism _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        private const string SpecialismName = "Specialism Updated";
        private const string LarId = "999";
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            var specialisms = new TlSpecialismBuilder().BuildList(_awardingOrganisation);
            _data = specialisms.FirstOrDefault();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.Name = SpecialismName;
            _data.LarId = LarId;
            _data.ModifiedBy = ModifiedBy;
        }

        public override void When()
        {
            Repository.UpdateAsync(_data).GetAwaiter().GetResult();
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
