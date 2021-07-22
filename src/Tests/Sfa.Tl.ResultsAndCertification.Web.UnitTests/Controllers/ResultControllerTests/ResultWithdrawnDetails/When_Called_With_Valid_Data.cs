using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ResultWithdrawnDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ResultDetailsViewModel _mockResult = null;

        public override void Given()
        {
            _mockResult = new ResultDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.Now.AddYears(-30),
                TlevelTitle = "Tlevel title",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567891,
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayAssessmentId = 11,
                PathwayResult = "A",
                PathwayResultId = 123,
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            ResultLoader.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
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

            model.Uln.Should().Be(_mockResult.Uln);
            model.Firstname.Should().Be(_mockResult.Firstname);
            model.Lastname.Should().Be(_mockResult.Lastname);
            model.LearnerName.Should().Be($"{_mockResult.Firstname} {_mockResult.Lastname}");
            model.DateofBirth.Should().Be(_mockResult.DateofBirth);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.TlevelTitle.Should().Be(_mockResult.TlevelTitle);
            model.ProviderDisplayName.Should().Be($"{_mockResult.ProviderName}<br/>({_mockResult.ProviderUkprn})");
            model.PathwayDisplayName.Should().Be(_mockResult.PathwayDisplayName);
            model.PathwayAssessmentSeries.Should().Be(_mockResult.PathwayAssessmentSeries);
            model.PathwayAssessmentId.Should().Be(_mockResult.PathwayAssessmentId);
            model.PathwayResult.Should().Be(_mockResult.PathwayResult);
            model.PathwayStatus.Should().Be(_mockResult.PathwayStatus);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.ResultsDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Result_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchResults);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Results);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Learners_Results);
        }
    }
}
