using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Qualification
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.Qualification>
    {
        private Domain.Models.Qualification _result;
        private Domain.Models.Qualification _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new QualificationBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Code = "100/5/99";
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
            _result.QualificationTypeId.Should().Be(_data.QualificationTypeId);
            _result.TlLookupId.Should().Be(_data.TlLookupId);
            _result.Code.Should().Be(_data.Code);
            _result.Title.Should().Be(_data.Title);
            _result.IsSendQualification.Should().Be(_data.IsSendQualification);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
