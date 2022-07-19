using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.OverallGradeLookup
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.OverallGradeLookup>
    {
        private Domain.Models.OverallGradeLookup _result;
        private Domain.Models.OverallGradeLookup _data;

        public override void Given()
        {
            _data = new OverallGradeLookupBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlLookupCoreGradeId.Should().Be(_data.TlLookupCoreGradeId);
            _result.TlLookupSpecialismGradeId.Should().Be(_data.TlLookupSpecialismGradeId);
            _result.TlLookupOverallGradeId.Should().Be(_data.TlLookupOverallGradeId);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
