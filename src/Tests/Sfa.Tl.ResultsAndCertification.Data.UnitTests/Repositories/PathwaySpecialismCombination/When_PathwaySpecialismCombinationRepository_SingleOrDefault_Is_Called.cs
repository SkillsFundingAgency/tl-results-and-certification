﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_PathwaySpecialismCombinationRepository_SingleOrDefault_Is_Called : BaseTest<TlPathwaySpecialismCombination>
    {
        private TlPathwaySpecialismCombination _result;
        private TlPathwaySpecialismCombination _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetSingleOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.PathwayId.Should().Be(_data.PathwayId);
            _result.SpecialismId.Should().Be(_data.SpecialismId);
            _result.Group.Should().Be(_data.Group);
            _result.PathwayId.Should().Be(_data.PathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
