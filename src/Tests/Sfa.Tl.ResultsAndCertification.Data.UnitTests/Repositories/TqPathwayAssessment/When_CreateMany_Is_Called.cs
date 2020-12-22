using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqPathwayAssessment
{
    public class When_CreateMany_Is_Called : BaseTest<Domain.Models.TqPathwayAssessment>
    {
        private int _result;
        private IList<Domain.Models.TqPathwayAssessment> _data;

        public override void Given()
        {
            _data = new TqPathwayAssessmentBuilder().BuildList();
        }

        public async override Task When()
        {
            _result = await Repository.CreateManyAsync(_data);
        }

        [Fact]
        public void Then_Records_Should_Have_Been_Created()
        {
            var result = Repository.GetManyAsync();
            result.Count().Should().Be(_data.Count);
        }
    }
}
