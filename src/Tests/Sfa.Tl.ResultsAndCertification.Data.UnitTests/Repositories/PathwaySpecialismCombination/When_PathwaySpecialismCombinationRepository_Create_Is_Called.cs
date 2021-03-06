﻿using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_PathwaySpecialismCombinationRepository_Create_Is_Called : BaseTest<TlPathwaySpecialismCombination>
    {
        private TlPathwaySpecialismCombination _data;
        private int _result;

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().Build();
        }

        public override void When()
        {
            _result = Repository.CreateAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
            _result.Should().Be(1);
    }
}
