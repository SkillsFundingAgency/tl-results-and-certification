﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Batch
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.Batch>
    {
        private Domain.Models.Batch _result;
        private Domain.Models.Batch _data;

        public override void Given()
        {
            _data = new BatchBuilder().Build();
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
            _result.Type.Should().Be(_data.Type);
            _result.Status.Should().Be(_data.Status);
            _result.Errors.Should().Be(_data.Errors);
            _result.PrintingStatus.Should().Be(_data.PrintingStatus);
            _result.RunOn.Should().Be(_data.RunOn);
            _result.StatusChangedOn.Should().Be(_data.StatusChangedOn);
            _result.ResponseStatus.Should().Be(_data.ResponseStatus);
            _result.ResponseMessage.Should().Be(_data.ResponseMessage);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
