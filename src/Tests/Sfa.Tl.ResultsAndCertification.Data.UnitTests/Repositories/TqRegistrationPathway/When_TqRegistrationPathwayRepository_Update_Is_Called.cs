using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationPathway
{
    public class When_TqRegistrationPathwayRepository_Update_Is_Called : BaseTest<Domain.Models.TqRegistrationPathway>
    {
        private Domain.Models.TqRegistrationPathway _result;
        private Domain.Models.TqRegistrationPathway _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqRegistrationPathwayBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TqProviderId = 2;
            _data.ModifiedBy = ModifiedBy;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TqRegistrationProfileId.Should().Be(_data.TqRegistrationProfileId);
            _result.TqProviderId.Should().Be(_data.TqProviderId);
            _result.AcademicYear.Should().Be(_data.AcademicYear);
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
