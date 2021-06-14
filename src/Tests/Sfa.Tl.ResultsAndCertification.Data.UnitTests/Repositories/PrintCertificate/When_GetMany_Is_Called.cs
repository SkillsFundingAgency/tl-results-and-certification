using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PrintCertificate
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.PrintCertificate>
    {
        private IEnumerable<Domain.Models.PrintCertificate> _result;
        private IList<Domain.Models.PrintCertificate> _data;

        public override void Given()
        {
            _data = new PrintCertificateBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Records_Are_Returned() =>
            _result.Count().Should().Be(_data.Count);

        [Fact]
        public void Then_First_Record_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.PrintBatchItemId.Should().Be(testData.PrintBatchItem.Id);
            result.TqRegistrationPathwayId.Should().Be(testData.TqRegistrationPathway.Id);
            result.Uln.Should().Be(testData.Uln);
            result.LearnerName.Should().Be(testData.LearnerName);
            result.Type.Should().Be(testData.Type);
            result.LearningDetails.Should().Be(testData.LearningDetails);
            result.DisplaySnapshot.Should().Be(testData.DisplaySnapshot);
            result.Status.Should().Be(testData.Status);
            result.Reason.Should().Be(testData.Reason);
            result.TrackingId.Should().Be(testData.TrackingId);
            result.StatusChangedOn.Should().Be(testData.StatusChangedOn);
            result.CreatedBy.Should().Be(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
