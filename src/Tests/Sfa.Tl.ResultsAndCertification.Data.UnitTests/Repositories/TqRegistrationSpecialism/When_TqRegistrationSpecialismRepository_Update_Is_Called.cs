using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationSpecialism
{
    public class When_TqRegistrationSpecialismRepository_Update_Is_Called : BaseTest<Domain.Models.TqRegistrationSpecialism>
    {
        private Domain.Models.TqRegistrationSpecialism _result;
        private Domain.Models.TqRegistrationSpecialism _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqRegistrationSpecialismBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TlSpecialismId = 3;
            _data.ModifiedBy = ModifiedBy;
        }

        public override void When()
        {
            Repository.UpdateAsync(_data).GetAwaiter().GetResult();
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id).GetAwaiter().GetResult();
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
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
