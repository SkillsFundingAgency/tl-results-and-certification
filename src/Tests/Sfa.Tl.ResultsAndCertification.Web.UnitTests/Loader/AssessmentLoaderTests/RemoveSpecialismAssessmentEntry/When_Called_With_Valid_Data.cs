using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.RemoveSpecialismAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private bool ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                SpecialismLarId = "ZT1234567|ZO565745",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 1,
                        LarId = "ZT1234567",
                        Name = "Specialism 1",
                        DisplayName = "Specialism 1 (ZT1234567)"
                    },
                    new SpecialismViewModel
                    {
                        Id = 2,
                        LarId = "ZO565745",
                        Name = "Specialism 2",
                        DisplayName = "Specialism 2 (ZO565745)"
                    },
                }
            };

            ExpectedApiResult = true;
            InternalApiClient
                .RemoveAssessmentEntryAsync(Arg.Is<RemoveAssessmentEntryRequest>(
                    x => x.SpecialismAssessmentIds.All(s => new List<int?> { 1, 2 }.Contains(s)) &&
                    x.AoUkprn == AoUkprn &&
                    x.ComponentType == Common.Enum.ComponentType.Specialism))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
