using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public class When_Called_With_RequestReplacementDocumentViewModel : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        protected RequestReplacementDocumentViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProviderUkprn = 9874561231;
            ProfileId = 1;

            _expectedApiResult = new Models.Contracts.TrainingProvider.LearnerRecordDetails
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 222,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = ProviderUkprn,
                TlevelTitle = "Course name (4561237)",
                AcademicYear = 2020,
                AwardingOrganisationName = "Pearson",
                IsLearnerRegistered = true,
                IndustryPlacementId = 1,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed,
                PrintCertificateId = 99,
                PrintCertificateType = Common.Enum.PrintCertificateType.Certificate,
                LastDocumentRequestedDate = DateTime.UtcNow.AddDays(-30),
                IsReprint = true,
                ProviderAddress = new Models.Contracts.ProviderAddress.Address { AddressId = 1, AddressLine1 = "Address1", AddressLine2 = "Address2", DepartmentName = "Dept", Town = "Birmingham", Postcode = "A1 2BC" }
            };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.Name);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.PrintCertificateId.Should().Be(_expectedApiResult.PrintCertificateId);
            ActualResult.PrintCertificateType.Should().Be(_expectedApiResult.PrintCertificateType);
            ActualResult.LastDocumentRequestedDate.Should().Be(_expectedApiResult.LastDocumentRequestedDate);
            ActualResult.ProviderAddress.Should().BeEquivalentTo(_expectedApiResult.ProviderAddress);

            ActualResult.ProviderAddress.ToDisplayValue.Should().Be("Address1<br/>Address2<br/>Birmingham<br/>A1 2BC<br/>");
        }
    }
}
