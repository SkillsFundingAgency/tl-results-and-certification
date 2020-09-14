using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DocumentUploadHistory
{
    public class When_DocumentUploadHistoryRepository_GetMany_Is_Called : BaseTest<Domain.Models.DocumentUploadHistory>
    {
        private IEnumerable<Domain.Models.DocumentUploadHistory> _result;
        private IList<Domain.Models.DocumentUploadHistory> _data;

        public override void Given()
        {
            _data = new DocumentUploadHistoryBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TlAwardingOrganisationId.Should().Be(testData.TlAwardingOrganisationId);
            result.BlobFileName.Should().BeEquivalentTo(testData.BlobFileName);
            result.BlobUniqueReference.Should().Be(testData.BlobUniqueReference);
            result.DocumentType.Should().Be(testData.DocumentType);
            result.FileType.Should().Be(testData.FileType);
            result.Status.Should().Be(testData.Status);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
