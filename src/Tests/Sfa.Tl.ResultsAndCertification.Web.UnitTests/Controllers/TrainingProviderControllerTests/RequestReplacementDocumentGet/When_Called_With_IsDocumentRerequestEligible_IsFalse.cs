using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentGet
{
    public class When_Called_With_IsDocumentRerequestEligible_IsFalse : TestSetup
    {
        private RequestReplacementDocumentViewModel _requestReplacementDocumentViewModel;

        public override void Given()
        {
            ProfileId = 1;
            _requestReplacementDocumentViewModel = new RequestReplacementDocumentViewModel { ProfileId = ProfileId, Uln = 987456123, PrintCertificateId = 1, 
                LastDocumentRequestedDate = DateTime.UtcNow.AddDays(-1), PrintCertificateType = Common.Enum.PrintCertificateType.Certificate, 
                ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1, AddressLine1 = "Address1", AddressLine2 = "Address2", DepartmentName = "Dept", Town = "Birmingham", Postcode = "A1 2BC" } };
            
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId).Returns(_requestReplacementDocumentViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
