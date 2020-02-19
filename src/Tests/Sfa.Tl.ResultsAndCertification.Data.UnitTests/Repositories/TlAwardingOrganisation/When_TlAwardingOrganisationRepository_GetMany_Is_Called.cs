using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlAwardingOrganisation
{
    public class When_TlAwardingOrganisationRepository_GetMany_Is_Called : BaseTest<Domain.Models.TlAwardingOrganisation>
    {
        private IEnumerable<Domain.Models.TlAwardingOrganisation> _result;
        private IList<Domain.Models.TlAwardingOrganisation> _data;

        public override void Given()
        {
            _data = new TlAwardingOrganisationBuilder().BuildList();
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
            _result.Count().Should().Be(2);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();

            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.Name.Should().Be(expectedResult.Name);
            actualResult.DisplayName.Should().Be(expectedResult.DisplayName);
            actualResult.UkPrn.Should().Be(expectedResult.UkPrn);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
