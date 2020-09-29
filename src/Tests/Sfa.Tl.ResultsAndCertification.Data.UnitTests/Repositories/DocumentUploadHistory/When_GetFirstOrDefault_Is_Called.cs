using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DocumentUploadHistory
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.DocumentUploadHistory>
    {
        private Domain.Models.DocumentUploadHistory _result;
        private Domain.Models.DocumentUploadHistory _data;

        public override void Given()
        {
            _data = new DocumentUploadHistoryBuilder().Build();
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
            _result.TlAwardingOrganisationId.Should().Be(_data.TlAwardingOrganisationId);
            _result.BlobFileName.Should().BeEquivalentTo(_data.BlobFileName);
            _result.BlobUniqueReference.Should().Be(_data.BlobUniqueReference);
            _result.DocumentType.Should().Be(_data.DocumentType);
            _result.FileType.Should().Be(_data.FileType);
            _result.Status.Should().Be(_data.Status);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
