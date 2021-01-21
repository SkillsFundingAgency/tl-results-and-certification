using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Collections.Generic;
using Xunit;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultDetails
{
    public class When_No_Result_Added : TestSetup
    {
        private ResultDetailsViewModel mockresult = null;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            mockresult = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayStatus = RegistrationPathwayStatus.Active
            };
            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, mockresult.ProfileId.ToString() } };
            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ResultDetailsViewModel));

            var model = viewResult.Model as ResultDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SpecialismDisplayName.Should().Be(mockresult.SpecialismDisplayName);
            model.PathwayStatus.Should().Be(mockresult.PathwayStatus);

            // Summary Core Result            
            model.SummaryCoreResult.Should().NotBeNull();
            model.SummaryCoreResult.Title.Should().Be(ResultDetailsContent.Title_Result_Text);
            model.SummaryCoreResult.Value.Should().Be(mockresult.PathwayAssessmentSeries);
            model.SummaryCoreResult.Value2.Should().Be(string.Format(ResultDetailsContent.Grade_Label_Text, ResultDetailsContent.Not_Specified_Text));
            model.SummaryCoreResult.ActionText.Should().Be(ResultDetailsContent.Add_Result_Action_Link_Text);
            model.SummaryCoreResult.RenderHiddenActionText.Should().Be(true);
            model.SummaryCoreResult.HiddenActionText.Should().Be(ResultDetailsContent.Core_Result_Hidden_Text);
            model.SummaryCoreResult.RouteName.Should().Be(RouteConstants.AddCoreResult);
            model.SummaryCoreResult.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
