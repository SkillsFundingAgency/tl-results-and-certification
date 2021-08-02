using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PrintCertificate
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.PrintCertificate>
    {
        private Domain.Models.PrintCertificate _result;
        private Domain.Models.PrintCertificate _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new PrintCertificateBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.DisplaySnapshot = "New Text";
            _data.ModifiedOn = DateTime.UtcNow;
            _data.ModifiedBy = ModifiedUserName;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
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
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
