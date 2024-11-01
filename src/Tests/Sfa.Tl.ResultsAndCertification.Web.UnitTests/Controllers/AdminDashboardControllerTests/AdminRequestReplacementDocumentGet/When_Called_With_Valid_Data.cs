using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly AdminRequestReplacementDocumentViewModel _mockResult = new()
        {
            RegistrationPathwayId = 150,
            Uln = 1234567890,
            LearnerName = "Test leaner name",
            ProviderName = "Barnsley College",
            ProviderUkprn = 10000536,
            ProviderAddress = new AddressViewModel
            {
                AddressId = 1,
                DepartmentName = "Operations",
                OrganisationName = "College Ltd",
                AddressLine1 = "10, House",
                AddressLine2 = "Street",
                Town = "Birmingham",
                Postcode = "B1 1AA"
            },
            PrintCertificateId = 1000,
            PrintCertificateType = PrintCertificateType.StatementOfAchievement,
            LastDocumentRequestedDate = new DateTime(2024, 1, 1)
        };

        public override void Given()
        {
            AdminDashboardLoader
                .GetAdminLearnerRecordAsync<AdminRequestReplacementDocumentViewModel>(RegistrationPathwayId)
                .Returns(_mockResult);

            AdminDashboardLoader.
                IsDocumentRerequestEligible(ResultsAndCertificationConfiguration.DocumentRerequestInDays, _mockResult.LastDocumentRequestedDate)
                .Returns(true);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var model = Result.ShouldBeViewResult<AdminRequestReplacementDocumentViewModel>();

            model.RegistrationPathwayId.Should().Be(_mockResult.RegistrationPathwayId);
            model.Uln.Should().Be(_mockResult.Uln);
            model.LearnerName.Should().Be(_mockResult.LearnerName);
            model.ProviderName.Should().Be(_mockResult.ProviderName);
            model.ProviderUkprn.Should().Be(_mockResult.ProviderUkprn);
            model.ProviderAddress.Should().Be(_mockResult.ProviderAddress);
            model.PrintCertificateId.Should().Be(_mockResult.PrintCertificateId);
            model.PrintCertificateType.Should().Be(_mockResult.PrintCertificateType);
            model.LastDocumentRequestedDate.Should().Be(_mockResult.LastDocumentRequestedDate);

            SummaryItemModel summaryDocumentRequested = model.SummaryDocumentRequested;
            summaryDocumentRequested.Id.Should().Be("requestDocumentType");
            summaryDocumentRequested.Title.Should().Be(AdminRequestReplacementDocument.Title_Document_Requested);
            summaryDocumentRequested.Value.Should().Be(AdminRequestReplacementDocument.Document_Statement_Of_Achievement);

            SummaryItemModel summaryProvider = model.SummaryProvider;
            summaryProvider.Id.Should().Be("provider");
            summaryProvider.Title.Should().Be(AdminRequestReplacementDocument.Title_Provider_Ukprn_Name_Text);
            summaryProvider.Value.Should().Be($"{_mockResult.ProviderName} ({_mockResult.ProviderUkprn})");

            SummaryItemModel summaryDepartment = model.SummaryDepartment;
            summaryDepartment.Id.Should().Be("departmentName");
            summaryDepartment.Title.Should().Be(AdminRequestReplacementDocument.Title_Department);
            summaryDepartment.Value.Should().Be(_mockResult.ProviderAddress.DepartmentName);

            SummaryItemModel summaryOrganisationAddress = model.SummaryOrganisationAddress;
            summaryOrganisationAddress.Id.Should().Be("organisationAddress");
            summaryOrganisationAddress.Title.Should().Be(AdminRequestReplacementDocument.Title_Organisation_Address);
            summaryOrganisationAddress.Value.Should().Be(_mockResult.ProviderAddress.ToDisplayValue);
            summaryOrganisationAddress.IsRawHtml.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
        }
    }
}