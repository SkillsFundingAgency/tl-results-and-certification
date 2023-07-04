using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialismToSpecialism
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<TlDualSpecialismToSpecialism>
    {
        private TlDualSpecialismToSpecialism _result;
        private IEnumerable<TlDualSpecialismToSpecialism> _data;

        public override void Given()
        {
            _data = new TlDualSpecialismToSpecialismBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault(x => x.Id == 1);

            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlDualSpecialismId.Should().Be(expectedResult.TlDualSpecialismId);
            _result.TlSpecialismId.Should().Be(expectedResult.TlSpecialismId);
            _result.CreatedBy.Should().Be(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().Be(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);

        }
    }
}
