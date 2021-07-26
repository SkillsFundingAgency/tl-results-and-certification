using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_Result_PrsStatus_Is_Final : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                PathwayStatus = RegistrationPathwayStatus.Active,

                PathwayAssessmentId = 11,
                PathwayResult = "A",
                AppealEndDate = DateTime.Today.AddDays(7),
                PathwayPrsStatus = PrsStatus.Final
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }
        [Fact]
        public void Then_Result_Status_Is_Final()
        {
            Result.Should().NotBeNull();
            var model = (Result as ViewResult).Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            // Summary CoreResult
            model.SummaryPathwayGrade.Should().NotBeNull();
            model.SummaryPathwayGrade.Title.Should().Be(ResultDetailsContent.Title_Pathway_Grade);
            model.SummaryPathwayGrade.Value.Should().Be(_mockResult.PathwayResult);
            model.SummaryPathwayGrade.Value2.Should().Be(CommonHelper.GetPrsStatusDisplayText(_mockResult.PathwayPrsStatus, _mockResult.AppealEndDate));
            model.SummaryPathwayGrade.Value2CustomCssClass.Should().Be(Constants.TagFloatRightClassName);
            model.SummaryPathwayGrade.RenderActionColumn.Should().Be(!_mockResult.IsValidPathwayPrsStatus);
            model.SummaryPathwayGrade.ActionText.Should().BeNull();
            model.SummaryPathwayGrade.RouteName.Should().BeNull();
            model.SummaryPathwayGrade.HiddenActionText.Should().BeNull();
            model.SummaryPathwayGrade.HiddenValueText.Should().BeNull();
            model.SummaryPathwayGrade.RouteAttributes.Should().BeNull();
        }
    }
}
