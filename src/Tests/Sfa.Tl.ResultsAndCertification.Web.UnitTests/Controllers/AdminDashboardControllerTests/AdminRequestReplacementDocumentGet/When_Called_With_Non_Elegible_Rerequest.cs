using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentGet
{
    public class When_Called_With_Non_Elegible_Rerequest : TestSetup
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
            LastDocumentRequestedDate = new DateTime(2024, 1, 1),
            IsCertificateRerequestEligible = false
        };

        public override void Given()
        {
            AdminDashboardLoader.
                GetAdminLearnerRecordAsync<AdminRequestReplacementDocumentViewModel>(RegistrationPathwayId)
                .Returns(_mockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}