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
    public class When_CreateMany_Is_Called : BaseTest<TlDualSpecialismToSpecialism>
    {
        private IList<TlDualSpecialismToSpecialism> _data;
        private int _result;

        public override void Given()
        {
            _data = new TlDualSpecialismToSpecialismBuilder().BuildList();
        }

        public async override Task When()
        {
            await Repository.CreateManyAsync(_data);
        }

        [Fact]
        public void Then_Expected_Record_Should_Have_Been_Created() 
        {
            var result = Repository.GetManyAsync();
            result.Count().Should().Be(_data.Count());
        }
    }
}
