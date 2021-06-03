using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IndustryPlacement
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.IndustryPlacement>
    {
        private Domain.Models.IndustryPlacement _result;
        private Domain.Models.IndustryPlacement _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new IndustryPlacementBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Status =  IndustryPlacementStatus.CompletedWithSpecialConsideration;
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
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathwayId);
            _result.Status.Should().Be(_data.Status);            
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
