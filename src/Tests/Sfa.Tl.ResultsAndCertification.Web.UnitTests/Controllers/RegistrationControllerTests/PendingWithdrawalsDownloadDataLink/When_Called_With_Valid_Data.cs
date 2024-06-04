using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using System.IO;
using System.Text;
using Xunit;
using RegistrationContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.PendingWithdrawalsDownloadDataLink
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly Guid _blobUniqueReference = new("f47d7a4e-9b8c-4a6f-8e4d-2e3b1a5c9f0d");

        public override void Given()
        {
            Id = _blobUniqueReference.ToString();
            RegistrationLoader.GetPendingWithdrawalsDataFileAsync(Ukprn, _blobUniqueReference).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetPendingWithdrawalsDataFileAsync(Ukprn, _blobUniqueReference);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(RegistrationContent.RegistrationsDownloadData.Pending_Withdrawals_Data_Report_File_Name);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
