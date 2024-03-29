﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.FunctionLog
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.FunctionLog>
    {
        private Domain.Models.FunctionLog _result;
        private Domain.Models.FunctionLog _data;

        public override void Given()
        {
            _data = new FunctionLogBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.FunctionType.Should().Be(_data.FunctionType);
            _result.Name.Should().Be(_data.Name);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.Status.Should().Be(_data.Status);
            _result.Message.Should().Be(_data.Message);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
