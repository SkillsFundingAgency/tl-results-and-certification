using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationPathway
{
    public class When_TqRegistrationPathway_CreateMany_Is_Called : BaseTest<Domain.Models.TqRegistrationPathway>
    {
        private int _result;
        private IList<Domain.Models.TqRegistrationPathway> _data;

        public override void Given()
        {
            _data = new TqRegistrationPathwayBuilder().BuildList();
        }

        public override void When()
        {
            _result = Repository.CreateManyAsync(_data)
                   .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(3);
    }
}
