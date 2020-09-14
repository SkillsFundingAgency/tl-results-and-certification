using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlProvider
{
    public class When_TlProviderRepository_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TlProvider>
    {
        private Domain.Models.TlProvider _result;
        private IEnumerable<Domain.Models.TlProvider> _data;

        public override void Given()
        {
            _data = new TlProviderBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
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
