using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationSpecialism
{
    public class When_TqRegistrationSpecialism_CreateMany_Is_Called : BaseTest<Domain.Models.TqRegistrationSpecialism>
    {
        private int _result;
        private IList<Domain.Models.TqRegistrationSpecialism> _data;

        public override void Given()
        {
            _data = new TqRegistrationSpecialismBuilder().BuildList();
        }

        public override void When()
        {
            _result = Repository.CreateManyAsync(_data)
                   .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Records_Should_Have_Been_Created()
        {
            var result = Repository.GetManyAsync();
            result.Count().Should().Be(_data.Count);
        }
    }
}
