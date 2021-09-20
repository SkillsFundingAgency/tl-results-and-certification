using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RegistrationDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RegistrationDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.RegistrationDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RegistrationDetailsViewModel mockresult = null;
        private Dictionary<string, string> _routeAttributes;
        private IList<AcademicYear> _academicYears;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, mockresult.ProfileId.ToString() } };
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };

            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId).Returns(mockresult);
            RegistrationLoader.GetCurrentAcademicYearsAsync().Returns(_academicYears);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(RegistrationDetailsViewModel));

            var model = viewResult.Model as RegistrationDetailsViewModel;
            model.Should().NotBeNull();

            model.Uln.Should().Be(mockresult.Uln);
            model.Name.Should().Be(mockresult.Name);
            model.DateofBirth.Should().Be(mockresult.DateofBirth);
            model.ProviderDisplayName.Should().Be(mockresult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.SpecialismsDisplayName.Should().BeEquivalentTo(mockresult.SpecialismsDisplayName);
            model.AcademicYear.Should().Be(mockresult.AcademicYear);
            model.Status.Should().Be(mockresult.Status);
            model.ShowAssessmentEntriesLink.Should().BeTrue();
            model.AcademicYears.Should().BeEquivalentTo(_academicYears);

            // Summary Status
            model.SummaryStatus.Should().NotBeNull();
            model.SummaryStatus.Title.Should().Be(RegistrationDetailsContent.Title_Status);
            model.SummaryStatus.Value.Should().Be(mockresult.Status.ToString());
            model.SummaryStatus.ActionText.Should().Be(RegistrationDetailsContent.Change_Status_Action_Link_Text);
            model.SummaryStatus.HasTag.Should().BeTrue();
            model.SummaryStatus.RenderHiddenActionText.Should().BeFalse();
            model.SummaryStatus.TagCssClass.Should().Be("govuk-tag--green");
            model.SummaryStatus.RouteName.Should().Be(RouteConstants.AmendActiveRegistration);
            model.SummaryStatus.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary LearnerName
            model.SummaryLearnerName.Should().NotBeNull();
            model.SummaryLearnerName.Title.Should().Be(RegistrationDetailsContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be(mockresult.Name);
            model.SummaryLearnerName.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);
            model.SummaryLearnerName.RouteName.Should().Be(RouteConstants.ChangeRegistrationLearnersName);
            model.SummaryLearnerName.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
            model.SummaryLearnerName.RenderHiddenActionText.Should().BeTrue();

            // Summary DateofBirth
            model.SummaryDateofBirth.Should().NotBeNull();
            model.SummaryDateofBirth.Title.Should().Be(RegistrationDetailsContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be(mockresult.DateofBirth.ToShortDateString());
            model.SummaryDateofBirth.RouteName.Should().Be(RouteConstants.ChangeRegistrationDateofBirth);
            model.SummaryDateofBirth.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);
            model.SummaryDateofBirth.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(RegistrationDetailsContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(mockresult.ProviderDisplayName);
            model.SummaryProvider.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);
            model.SummaryProvider.RouteName.Should().Be(RouteConstants.ChangeRegistrationProvider);
            model.SummaryProvider.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Core
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(RegistrationDetailsContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(mockresult.PathwayDisplayName);
            model.SummaryCore.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);
            model.SummaryCore.RouteName.Should().Be(RouteConstants.ChangeRegistrationCore);
            model.SummaryCore.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);            

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(RegistrationDetailsContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeEquivalentTo(mockresult.SpecialismsDisplayName);
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.ChangeRegistrationSpecialismQuestion);
            model.SummarySpecialisms.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);

            // Summary Academic Year
            model.SummaryAcademicYear.Should().NotBeNull();
            model.SummaryAcademicYear.Title.Should().Be(RegistrationDetailsContent.Title_AcademicYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(mockresult.GetAcademicYearName);
            model.SummaryAcademicYear.ActionText.Should().Be(RegistrationDetailsContent.Change_Action_Link_Text);
            model.SummaryAcademicYear.RouteName.Should().Be(RouteConstants.ChangeAcademicYear);
            model.SummaryAcademicYear.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(4);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.RegistrationDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Registration_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().Be(RouteConstants.SearchRegistration);
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Search_For_Registration);
            model.Breadcrumb.BreadcrumbItems[3].RouteName.Should().BeNullOrEmpty();
            model.Breadcrumb.BreadcrumbItems[3].DisplayName.Should().Be(BreadcrumbContent.Registration_Details);
        }
    }
}
