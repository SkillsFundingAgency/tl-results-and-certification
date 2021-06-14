using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PrintCertificate
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.PrintCertificate>
    {
        private Domain.Models.PrintCertificate _result;
        private Domain.Models.PrintCertificate _data;

        public override void Given()
        {
            _data = new PrintCertificateBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.PrintBatchItemId.Should().Be(_data.PrintBatchItem.Id);
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathway.Id);
            _result.Uln.Should().Be(_data.Uln);
            _result.LearnerName.Should().Be(_data.LearnerName);
            _result.Type.Should().Be(_data.Type);
            _result.LearningDetails.Should().Be(_data.LearningDetails);
            _result.DisplaySnapshot.Should().Be(_data.DisplaySnapshot);
            _result.Status.Should().Be(_data.Status);
            _result.Reason.Should().Be(_data.Reason);
            _result.TrackingId.Should().Be(_data.TrackingId);
            _result.StatusChangedOn.Should().Be(_data.StatusChangedOn);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
