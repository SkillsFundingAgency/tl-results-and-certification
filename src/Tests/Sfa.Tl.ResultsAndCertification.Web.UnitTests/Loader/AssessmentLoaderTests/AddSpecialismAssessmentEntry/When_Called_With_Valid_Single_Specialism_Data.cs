using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.AddSpecialismAssessmentEntry
{
    public class When_Called_With_Valid_Single_Specialism_Data : TestSetup
    {
        private AddAssessmentEntryResponse ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                AssessmentSeriesId = 1,
                SpecialismsId = "5",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            ExpectedApiResult = new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1234567890 };
            InternalApiClient
                .AddAssessmentEntryAsync(Arg.Is<AddAssessmentEntryRequest>(
                    x => x.ProfileId == ViewModel.ProfileId &&
                    x.AoUkprn == AoUkprn &&
                    x.ComponentType == Common.Enum.ComponentType.Specialism &&
                    x.AssessmentSeriesId == ViewModel.AssessmentSeriesId &&
                    x.SpecialismIds.All(s => new List<int?> { ViewModel.SpecialismsId.ToInt() }.Contains(s)) &&
                    x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(ExpectedApiResult.IsSuccess);
            ActualResult.Uln.Should().Be(ExpectedApiResult.Uln);
        }
    }
}
