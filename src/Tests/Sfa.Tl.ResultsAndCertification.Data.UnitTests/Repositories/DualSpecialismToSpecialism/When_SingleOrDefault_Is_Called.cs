using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialismToSpecialism
{
    public class When_SingleOrDefault_Is_Called : BaseTest<TlDualSpecialismToSpecialism>
    {
        private TlDualSpecialismToSpecialism _result;
        private TlDualSpecialismToSpecialism _data;

        public override void Given()
        {
            var tlDualSpecialisms = new TlDualSpecialismToSpecialismBuilder().BuildList();
            DbContext.AddRange(tlDualSpecialisms);
            DbContext.SaveChanges();
            _data = tlDualSpecialisms.FirstOrDefault();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_data.Id);
            _result.TlDualSpecialismId.Should().Be(_data.TlDualSpecialismId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
