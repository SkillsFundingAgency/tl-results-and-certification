using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using RequestReplacementDocumentContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.RequestReplacementDocument;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RequestReplacementDocumentViewModel _requestReplacementDocumentViewModel;

        public override void Given()
        {
            ProfileId = 1;
            _requestReplacementDocumentViewModel = new RequestReplacementDocumentViewModel { ProfileId = ProfileId, Uln = 987456123, PrintCertificateId = 1, LastDocumentRequestedDate = DateTime.UtcNow.AddDays(-22), PrintCertificateType = Common.Enum.PrintCertificateType.Certificate, 
                ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1, AddressLine1 = "Address1", AddressLine2 = "Address2", DepartmentName = "Dept", Town = "Birmingham", Postcode = "A1 2BC" } };
            
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId).Returns(_requestReplacementDocumentViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as RequestReplacementDocumentViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_requestReplacementDocumentViewModel.ProfileId);
            model.Uln.Should().Be(_requestReplacementDocumentViewModel.Uln);
            model.LearnerName.Should().Be(_requestReplacementDocumentViewModel.LearnerName);
            model.PrintCertificateId.Should().Be(_requestReplacementDocumentViewModel.PrintCertificateId);
            model.PrintCertificateType.Should().Be(_requestReplacementDocumentViewModel.PrintCertificateType);
            model.LastDocumentRequestedDate.Should().Be(_requestReplacementDocumentViewModel.LastDocumentRequestedDate);
            model.ProviderAddress.Should().BeEquivalentTo(_requestReplacementDocumentViewModel.ProviderAddress);

            // Summary Document Requested
            model.SummaryDocumentRequested.Should().NotBeNull();
            model.SummaryDocumentRequested.Title.Should().Be(RequestReplacementDocumentContent.Title_Document_Requested);
            model.SummaryDocumentRequested.Value.Should().Be(RequestReplacementDocumentContent.Document_Certificate);

            // Summary Department
            model.SummaryDepartment.Should().NotBeNull();
            model.SummaryDepartment.Title.Should().Be(RequestReplacementDocumentContent.Title_Department);
            model.SummaryDepartment.Value.Should().Be(_requestReplacementDocumentViewModel.ProviderAddress.DepartmentName);

            // Summary Organisation Address
            model.SummaryOrganisationAddress.Should().NotBeNull();
            model.SummaryOrganisationAddress.Title.Should().Be(RequestReplacementDocumentContent.Title_Organisation_Address);
            model.SummaryOrganisationAddress.Value.Should().Be(_requestReplacementDocumentViewModel.ProviderAddress.ToDisplayValue);
            model.SummaryOrganisationAddress.IsRawHtml.Should().BeTrue();

            // Breadcrumb
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(2);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);

            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Learners_Record);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.Breadcrumb.BreadcrumbItems[1].RouteAttributes.Should().HaveCount(1);
            model.Breadcrumb.BreadcrumbItems[1].RouteAttributes[Constants.ProfileId].Should().Be(ProfileId.ToString());
        }
    }
}
