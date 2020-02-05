using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_MandatoryAdditionalRequirement_Update_Is_Called : BaseTest<TlMandatoryAdditionalRequirement>
    {
        private TlMandatoryAdditionalRequirement _result;
        private TlMandatoryAdditionalRequirement _data;

        private const string MandatoryAdditionalRequirementName = "MandatoryAdditionalRequirementName Updated";
        private const string LarId = "999";
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlMandatoryAdditionalRequirementBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.Name = MandatoryAdditionalRequirementName;
            _data.IsRegulatedQualification = false;
            _data.LarId = LarId;
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
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.IsRegulatedQualification.Should().Be(_data.IsRegulatedQualification);
            _result.LarId.Should().BeEquivalentTo(_data.LarId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
