using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsDataForLink
{
    public class When_BlobUniqueIdentifier_Is_Guid : TestSetup
    {
        private const string GuidString = "70ecb1ae-cabc-41a6-8307-689fdba26820";
        private readonly Guid Guid = new(GuidString);

        private readonly FileStreamResult _fileStreamResult = new(new MemoryStream(), "text/csv")
        {
            FileDownloadName = "test-file-download.csv"
        };

        public override void Given()
        {
            ProviderRegistrationsLoader.GetRegistrationsFileAsync(ProviderUkprn, Guid).Returns(_fileStreamResult);
        }

        public override async Task When()
        {
            await When(GuidString);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType<FileStreamResult>()
                .And.BeEquivalentTo(_fileStreamResult);
        }
    }
}