using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentWithdrawnDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private AssessmentUlnWithdrawnViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentUlnWithdrawnViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname ="First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                TlevelTitle = "TLevel in Test",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentUlnWithdrawnViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(mockresult);         
        }        

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentUlnWithdrawnViewModel));

            var model = viewResult.Model as AssessmentUlnWithdrawnViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.LearnerName.Should().Be($"{mockresult.Firstname} {mockresult.Lastname}");
            model.DateofBirth.Should().Be(mockresult.DateofBirth);
            model.ProviderDisplayName.Should().Be($"{mockresult.ProviderName}<br/>({mockresult.ProviderUkprn})");
            model.TlevelTitle.Should().Be(mockresult.TlevelTitle);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.SearchAssessments);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.PopulateUln, out string routeValue);
            routeValue.Should().Be(true.ToString());
        }
    }
}
