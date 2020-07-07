using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationPathway
{
    public class When_TqRegistrationPathwayRepository_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TqRegistrationPathway>
    {
        private Domain.Models.TqRegistrationPathway _result;
        private Domain.Models.TqRegistrationPathway _data;

        public override void Given()
        {
            _data = new TqRegistrationPathwayBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
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
            _result.RegistrationDate.Should().Be(_data.RegistrationDate);
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
