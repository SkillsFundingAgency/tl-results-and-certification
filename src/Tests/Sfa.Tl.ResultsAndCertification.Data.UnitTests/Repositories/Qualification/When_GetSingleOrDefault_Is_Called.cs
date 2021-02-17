using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Qualification
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.Qualification>
    {
        private Domain.Models.Qualification _result;
        private Domain.Models.Qualification _data;

        public override void Given()
        {
            _data = new QualificationBuilder().Build();
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
            _result.QualificationTypeId.Should().Be(_data.QualificationTypeId);
            _result.TlLookupId.Should().Be(_data.TlLookupId);
            _result.Code.Should().Be(_data.Code);
            _result.Title.Should().Be(_data.Title);
            _result.IsSendQualification.Should().Be(_data.IsSendQualification);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
