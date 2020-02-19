
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_TqAwardingOrganisationRepository_GetMany_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private IEnumerable<TqAwardingOrganisation> _result;
        private IList<TqAwardingOrganisation> _data;

        public override void Given()
        {
            _data = new TqAwardingOrganisationBuilder().BuildList();
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
            _result.Count().Should().Be(1);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.TlAwardingOrganisatonId.Should().Be(expectedResult.TlAwardingOrganisatonId);
            actualResult.TlRouteId.Should().Be(expectedResult.TlRouteId);
            actualResult.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
