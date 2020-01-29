using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_PathRepository_CreateMany_Is_Called : BaseTest<TlPathway>
    {
        private IList<TlPathway> _data;
        private int _result;

        public override void Given()
        {
            _data = new TlPathwayBuilder().BuildList();
        }

        public override void When()
        {
            _result = Repository.CreateManyAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Record_Should_Have_Been_Created() =>
            _result.Should().Be(3);
    }
}
