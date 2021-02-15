using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ChangeCoreResult
{
    public class When_Mapper_Called : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewModel = new ManageCoreResultViewModel
            {
                Uln = 1234567890,
                ProfileId = ProfileId,
                ResultId = 1,
                SelectedGradeCode = "PCG1",
                LookupId = 1
            };

            var result = Mapper.Map<ChangeResultRequest>(viewModel, opt => opt.Items["aoUkprn"] = AoUkprn);

            result.Should().NotBeNull();
            result.AoUkprn.Should().Be(AoUkprn);
            result.Uln.Should().Be(viewModel.Uln);
            result.ProfileId.Should().Be(viewModel.ProfileId);
            result.ResultId.Should().Be(viewModel.ResultId);
            result.LookupId.Should().Be(viewModel.LookupId);
            result.ComponentType.Should().Be(ComponentType.Core);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
