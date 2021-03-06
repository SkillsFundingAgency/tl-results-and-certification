﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismMar
{
    public class When_PathwaySpecialismMarRepository_GetMany_Is_Called : BaseTest<TlPathwaySpecialismMar>
    {
        private IEnumerable<TlPathwaySpecialismMar> _result;
        private IEnumerable<TlPathwaySpecialismMar> _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismMarBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }


        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned()
        {
            _result.Count().Should().Be(8);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();
            
            expectedResult.Should().NotBeNull();
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.TlMandatoryAdditionalRequirementId.Should().Be(expectedResult.TlMandatoryAdditionalRequirementId);
            actualResult.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            actualResult.TlSpecialismId.Should().Be(expectedResult.TlSpecialismId);
            actualResult.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
