using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.OverallGradeLookup
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.OverallGradeLookup>
    {
        private Domain.Models.OverallGradeLookup _result;
        private Domain.Models.OverallGradeLookup _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new OverallGradeLookupBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.TlPathwayId = 2;
            _data.ModifiedOn = DateTime.UtcNow;
            _data.ModifiedBy = ModifiedUserName;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlLookupCoreGradeId.Should().Be(_data.TlLookupCoreGradeId);
            _result.TlLookupSpecialismGradeId.Should().Be(_data.TlLookupSpecialismGradeId);
            _result.TlLookupOverallGradeId.Should().Be(_data.TlLookupOverallGradeId);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
