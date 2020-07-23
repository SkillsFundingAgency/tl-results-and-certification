using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_TqRegistrationProfile_CreateMany_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private int _result;
        private IList<Domain.Models.TqRegistrationProfile> _data;

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().BuildList();
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
