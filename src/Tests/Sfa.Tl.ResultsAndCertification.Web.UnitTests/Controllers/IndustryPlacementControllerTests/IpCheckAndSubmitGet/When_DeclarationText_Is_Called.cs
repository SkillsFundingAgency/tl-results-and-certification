using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_DeclarationText_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, "John Smith")
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed } },
                        string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, "John Smith")
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.NotCompleted } },
                        CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held // TODO: rename these 
                    },
                    new object[]
                    {
                        new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.WillNotComplete } },
                        "TODO"
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(IndustryPlacementViewModel cacheModel, string expectedDeclarationText)
        {
            var viewModel = new IpCheckAndSubmitViewModel { ProfileId = 1, LearnerName = "John Smith" };
            viewModel.SetDeclarationText(cacheModel);

            viewModel.DeclarationText.Should().Be(expectedDeclarationText);
        }
    }
}
