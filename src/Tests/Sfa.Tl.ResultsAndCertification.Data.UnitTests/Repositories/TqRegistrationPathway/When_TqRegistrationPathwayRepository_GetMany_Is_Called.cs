using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationPathway
{
    public class When_TqRegistrationPathwayRepository_GetMany_Is_Called : BaseTest<Domain.Models.TqRegistrationPathway>
    {
        private IEnumerable<Domain.Models.TqRegistrationPathway> _result;
        private IList<Domain.Models.TqRegistrationPathway> _data;

        public override void Given()
        {
            _data = new TqRegistrationPathwayBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(3);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TqRegistrationProfileId.Should().Be(testData.TqRegistrationProfileId);
            result.TqProviderId.Should().Be(testData.TqProviderId);
            result.AcademicYear.Should().Be(testData.AcademicYear);
            result.StartDate.Should().Be(testData.StartDate);
            result.Status.Should().Be(testData.Status);
            result.IsBulkUpload.Should().Be(testData.IsBulkUpload);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
