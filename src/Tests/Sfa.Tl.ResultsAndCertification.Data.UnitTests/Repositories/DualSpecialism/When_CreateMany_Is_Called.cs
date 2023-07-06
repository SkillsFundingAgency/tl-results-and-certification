using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialism
{
    public class When_CreateMany_Is_Called : BaseTest<TlDualSpecialism>
    {
        private IList<TlDualSpecialism> _data;        
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            _data = new TlDualSpecialismBuilder().BuildList(_awardingOrganisation);
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
