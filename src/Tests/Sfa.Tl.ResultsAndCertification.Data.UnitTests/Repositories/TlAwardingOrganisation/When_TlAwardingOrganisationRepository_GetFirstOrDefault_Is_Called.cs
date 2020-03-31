using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlAwardingOrganisation
{
    public class When_TlAwardingOrganisationRepository_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TlAwardingOrganisation>
    {
        private Domain.Models.TlAwardingOrganisation _result;
        private IEnumerable<Domain.Models.TlAwardingOrganisation> _data;

        public override void Given()
        {
            _data = new TlAwardingOrganisationBuilder().BuildList();
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
            _result.Id.Should().Be(expectedResult.Id);
            _result.Name.Should().Be(expectedResult.Name);
            _result.DisplayName.Should().Be(expectedResult.DisplayName);
            _result.UkPrn.Should().Be(expectedResult.UkPrn);
            _result.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
