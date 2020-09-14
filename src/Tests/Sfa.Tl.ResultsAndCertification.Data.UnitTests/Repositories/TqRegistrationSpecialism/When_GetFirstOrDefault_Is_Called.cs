using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationSpecialism
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TqRegistrationSpecialism>
    {
        private Domain.Models.TqRegistrationSpecialism _result;
        private Domain.Models.TqRegistrationSpecialism _data;

        public override void Given()
        {
            _data = new TqRegistrationSpecialismBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathwayId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.Status.Should().Be(_data.Status);
            _result.IsBulkUpload.Should().Be(_data.IsBulkUpload);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
