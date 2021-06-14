using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DocumentUploadHistory
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.DocumentUploadHistory>
    {
        private Domain.Models.DocumentUploadHistory _result;
        private Domain.Models.DocumentUploadHistory _data;
        private const BulkProcessStatus UpdateStatus = BulkProcessStatus.Failed;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new DocumentUploadHistoryBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Status = (int)UpdateStatus;
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
            _result.TlAwardingOrganisationId.Should().Be(_data.TlAwardingOrganisationId);
            _result.BlobFileName.Should().Be(_data.BlobFileName);
            _result.BlobUniqueReference.Should().Be(_data.BlobUniqueReference);
            _result.DocumentType.Should().Be(_data.DocumentType);
            _result.FileType.Should().Be(_data.FileType);
            _result.Status.Should().Be(_data.Status);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
