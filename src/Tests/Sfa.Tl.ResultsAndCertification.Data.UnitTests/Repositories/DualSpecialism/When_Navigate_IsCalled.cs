﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialism
{
    public class When_Navigate_IsCalled : BaseTest<TlDualSpecialism>
    {
        private TlDualSpecialism _result;
        private TlDualSpecialism _data;
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            var tlDualSpecialisms = new TlDualSpecialismBuilder().BuildList(_awardingOrganisation);
            DbContext.AddRange(tlDualSpecialisms);
            DbContext.SaveChanges();
            _data = tlDualSpecialisms.FirstOrDefault();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.TlPathway.Should().NotBeNull();
        }

        [Fact]
        public void Then_Pathway_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlPathway;
            
            _result.TlPathway.Id.Should().Be(expectedResult.Id);
            _result.TlPathway.Name.Should().Be(expectedResult.Name);
            _result.TlPathway.LarId.Should().Be(expectedResult.LarId);
            _result.TlPathway.CreatedBy.Should().Be(expectedResult.CreatedBy);
            _result.TlPathway.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.TlPathway.ModifiedBy.Should().Be(expectedResult.ModifiedBy);
            _result.TlPathway.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }        
    }
}
