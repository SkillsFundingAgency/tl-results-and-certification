using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_PathwayRepository_Navigate_Is_Called : BaseTest<TlPathway>
    {
        private TlPathway _result;
        private TlPathway _data;

        public override void Given()
        {
            _data = new TlPathwayBuilder().Build();
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
            _result.Should().NotBeNull();
            _result.Route.Should().NotBeNull();

            var expectedResult = _data.Route;
            _result.Route.Id.Should().Be(expectedResult.Id);
            _result.Route.Name.Should().Be(expectedResult.Name);
            _result.Route.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.Route.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.Route.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.Route.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
