using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddResult
{
    public class When_Mapper_Called : TestSetup
    {
        public override void Given() {}

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewModel = new AddCoreResultViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = 1,
                SelectedGradeCode = "PCG1",
                TlLookupId = 1
            };

            var result = Mapper.Map<AddResultRequest>(viewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

            result.Should().NotBeNull();
            result.AoUkprn.Should().Be(AoUkprn);
            result.ProfileId.Should().Be(viewModel.ProfileId);
            result.TqPathwayAssessmentId.Should().Be(viewModel.AssessmentId);
            result.TlLookupId.Should().Be(viewModel.TlLookupId);
            result.AssessmentEntryType.Should().Be(AssessmentEntryType.Core);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
