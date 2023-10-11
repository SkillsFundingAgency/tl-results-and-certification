using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_DeclarationText_Is_Called
    {
        private const string LearnerName = "John Smith";
        private const int AcademicYear = 2023;
        private const int CompletionAcademicYear = AcademicYear + 2;

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration, AcademicYear = AcademicYear, CompletionAcademicYear = CompletionAcademicYear } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName, CompletionAcademicYear)
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed, AcademicYear = AcademicYear, CompletionAcademicYear = CompletionAcademicYear } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName, CompletionAcademicYear)
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.NotCompleted } },
                        CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.WillNotComplete } },
                         CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementViewModel cacheModel, string expectedDeclarationConfirmText)
        {
            var viewModel = new IpCheckAndSubmitViewModel { ProfileId = 1, LearnerName = LearnerName };
            viewModel.SetDeclarationText(cacheModel);

            viewModel.DeclarationConfirmSupportText.Should().Be(expectedDeclarationConfirmText);
        }
    }
}
