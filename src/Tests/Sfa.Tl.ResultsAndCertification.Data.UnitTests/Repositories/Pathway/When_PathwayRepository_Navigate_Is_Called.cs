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
            _result.TlRoute.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlRoute;
            
            _result.TlRoute.Id.Should().Be(expectedResult.Id);
            _result.TlRoute.Name.Should().Be(expectedResult.Name);
            _result.TlRoute.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.TlRoute.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.TlRoute.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.TlRoute.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
