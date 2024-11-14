using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.CommonHelperTests
{
    public class When_IsDocumentRerequestEligible_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Object params { documentRerequestInDays, printCertificateId, lastPrintRequestedDate, expectedResult}
                    new object[] { 21, 1, null, true },
                    new object[] { 21, null, null, false },
                    new object[] { 21, 1, DateTime.UtcNow, false },
                    new object[] { 21, 1, DateTime.UtcNow.AddDays(-21), false },
                    new object[] { 21, 1, DateTime.UtcNow.AddDays(-22), true },
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(int documentRerequestInDays, int? printCertificateId, DateTime? lastPrintRequestedDate, bool expectedResult)
        {
            var actualResult = CommonHelper.IsDocumentRerequestEligible(documentRerequestInDays, printCertificateId, lastPrintRequestedDate);
            actualResult.Should().Be(expectedResult);
        }
    }
}