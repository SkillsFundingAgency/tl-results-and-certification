﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_PathwaySpecialismCombinationRepository_GetFirstOrDefault_Is_Called : BaseTest<TlPathwaySpecialismCombination>
    {
        private TlPathwaySpecialismCombination _result;
        private IEnumerable<TlPathwaySpecialismCombination> _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault(x => x.Id == 1);

            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            _result.TlSpecialismId.Should().Be(expectedResult.TlSpecialismId);
            _result.Group.Should().Be(expectedResult.Group);
            _result.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
