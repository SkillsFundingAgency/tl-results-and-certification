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
        private const int CompletionYear = AcademicYear + 2;

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration, AcademicYear = AcademicYear, CompletionYear = CompletionYear } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName, CompletionYear),
                        string.Format(CheckAndSubmitContent.Declaration_I_Will_Make_Sure_The_Record_Is_Updated, CompletionYear)
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed, AcademicYear = AcademicYear, CompletionYear = CompletionYear } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName, CompletionYear),
                        string.Format(CheckAndSubmitContent.Declaration_I_Will_Make_Sure_The_Record_Is_Updated, CompletionYear)
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.NotCompleted } },
                        CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held,
                        null
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.WillNotComplete } },
                         CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held,
                         null
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementViewModel cacheModel, string expectedDeclarationConfirmText, string expectedDeclarationUpdateText)
        {
            var viewModel = new IpCheckAndSubmitViewModel { ProfileId = 1, LearnerName = LearnerName };
            viewModel.SetDeclarationText(cacheModel);

            viewModel.DeclarationConfirmSupportText.Should().Be(expectedDeclarationConfirmText);
            viewModel.DeclarationUpdateRecordText.Should().Be(expectedDeclarationUpdateText);
        }
    }
}
