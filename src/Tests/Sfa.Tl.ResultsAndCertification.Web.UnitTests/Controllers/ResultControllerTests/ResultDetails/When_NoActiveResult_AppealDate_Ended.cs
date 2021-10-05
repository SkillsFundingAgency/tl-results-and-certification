using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_NoActiveResult_AppealDate_Ended : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;
        private Dictionary<string, string> _routeAttributes;
        
        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                PathwayStatus = RegistrationPathwayStatus.Active,

                ProfileId = 1,
                PathwayResultId = 0,
                PathwayAssessmentId = 11,
                PathwayResult = "A",
                AppealEndDate = DateTime.Today.AddDays(-7),
                PathwayPrsStatus = null
            };

            _routeAttributes = new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, _mockResult.PathwayAssessmentId.ToString() }
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockResult);
        }

        [Fact]
        public void Then_Result_Change_IsAllowed()
        {
            Result.Should().NotBeNull();
            var model = (Result as ViewResult).Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            // Summary CoreResult
            model.SummaryPathwayGrade.Should().NotBeNull();
            model.SummaryPathwayGrade.Title.Should().Be(ResultDetailsContent.Title_Pathway_Grade);
            model.SummaryPathwayGrade.Value.Should().Be(_mockResult.PathwayResult);
            model.SummaryPathwayGrade.Value2.Should().BeNull();
            model.SummaryPathwayGrade.Value2CustomCssClass.Should().BeNull();
            model.SummaryPathwayGrade.RenderActionColumn.Should().Be(_mockResult.IsResultAddOrChangeAllowed);
            model.SummaryPathwayGrade.ActionText.Should().Be(ResultDetailsContent.Change_Result_Action_Link_Text);
            model.SummaryPathwayGrade.RenderHiddenActionText.Should().Be(true);
            model.SummaryPathwayGrade.HiddenActionText.Should().Be(ResultDetailsContent.Hidden_Action_Text_Core);
            model.SummaryPathwayGrade.HiddenValueText.Should().Be(ResultDetailsContent.Hidden_Value_Text_For);
            model.SummaryPathwayGrade.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
