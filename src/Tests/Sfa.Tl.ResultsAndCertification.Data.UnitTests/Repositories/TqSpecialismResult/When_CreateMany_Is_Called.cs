using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqSpecialismResult
{
    public class When_CreateMany_Is_Called : BaseTest<Domain.Models.TqSpecialismResult>
    {        
        private IList<Domain.Models.TqSpecialismResult> _data;

        public override void Given()
        {
            _data = new TqSpecialismResultBuilder().BuildList();
        }

        public async override Task When()
        {
            await Repository.CreateManyAsync(_data);
        }

        [Fact]
        public void Then_Records_Should_Have_Been_Created()
        {
            var result = Repository.GetManyAsync();
            result.Count().Should().Be(_data.Count);
        }
    }
}
