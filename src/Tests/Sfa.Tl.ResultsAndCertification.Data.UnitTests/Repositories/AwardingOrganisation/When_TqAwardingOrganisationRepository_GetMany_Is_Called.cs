
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_TqAwardingOrganisationRepository_GetMany_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private IEnumerable<TqAwardingOrganisation> _result;
        private IEnumerable<TqAwardingOrganisation> _data;

        public override void Given()
        {
            _data = new List<TqAwardingOrganisation> { new TqAwardingOrganisation { Id = 1 } };
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
            //_result.Count().Should().Be(3);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
        }
    }
}
