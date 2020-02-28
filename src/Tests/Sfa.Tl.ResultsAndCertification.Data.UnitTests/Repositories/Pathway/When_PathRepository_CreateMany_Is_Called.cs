using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_PathRepository_CreateMany_Is_Called : BaseTest<TlPathway>
    {
        private IList<TlPathway> _data;
        private int _result;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlPathwayBuilder().BuildList(_awardingOrganisation);
        }

        public override void When()
        {
            _result = Repository.CreateManyAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Record_Should_Have_Been_Created()
        {
            var result = Repository.GetManyAsync();
            result.Count().Should().Be(2);
        }
    }
}
