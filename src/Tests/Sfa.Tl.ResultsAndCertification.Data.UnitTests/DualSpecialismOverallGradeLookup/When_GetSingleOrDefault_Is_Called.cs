using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialismOverallGradeLookup
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.DualSpecialismOverallGradeLookup>
    {
        private Domain.Models.DualSpecialismOverallGradeLookup _result;
        private Domain.Models.DualSpecialismOverallGradeLookup _data;

        public override void Given()
        {
            _data = new DualSpecialismOverallGradeLookupBuilder().Build();
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
            _result.FirstTlLookupSpecialismGradeId.Should().Be(_data.FirstTlLookupSpecialismGradeId);
            _result.SecondTlLookupSpecialismGradeId.Should().Be(_data.SecondTlLookupSpecialismGradeId);
            _result.TlLookupOverallSpecialismGradeId.Should().Be(_data.TlLookupOverallSpecialismGradeId);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}