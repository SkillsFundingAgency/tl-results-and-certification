using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AcademicYear = Sfa.Tl.ResultsAndCertification.Models.Contracts.Common.AcademicYear;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.IndustryPlacementNotificationServiceTests.ProcessIndustryPlacementMissedDeadlineReminder
{
    public class ProcessIndustryPlacementMissedDeadlineReminderBaseTest : IndustryPlacementNotificationServiceBaseTest
    {
        protected IndustryPlacementNotificationResponse Result;
        public override void Given()
        {
            RegistrationPathwayRepository.GetManyAsync().Returns(CreateRegistrationPathways());
            CommonRepository.GetCurrentAcademicYearsAsync().Returns(CreateAcademicYears());
            NotificationService.SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>()).Returns(true);
            DfeSignInApiClient.GetDfeUsersAllProviders(Arg.Any<List<long>>())
                .Returns(new List<DfeUsers>() { new DfeUsers() {
                        Ukprn = "10000001",
                        Users = new List<ServiceUser>() {
                         new ServiceUser() {
                            Email = "test@test.com"}
                        }
                    }
                });
        }

        public async override Task When()
        {
            Result = await IndustryPlacementNotificationService.ProcessIndustryPlacementMissedDeadlineReminderAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            RegistrationPathwayRepository.Received(1).GetManyAsync();
            CommonRepository.Received(1).GetCurrentAcademicYearsAsync();
            NotificationService.Received(1).SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
            DfeSignInApiClient.Received(1).GetDfeUsersAllProviders(Arg.Any<List<long>>());
        }

        [Fact]
        public void Then_Returns_Expected_Data()
        {
            Result.UsersCount.Equals(1);
            Result.EmailSentCount.Equals(1);
            Result.IsSuccess.Equals(true);
        }


        private IEnumerable<AcademicYear> CreateAcademicYears() =>
             new List<AcademicYear>
            {
                new AcademicYear { Id = 1, Year = 2021 },
                new AcademicYear { Id = 2, Year = 2022 },
                new AcademicYear { Id = 2, Year = 2023 }
            }.AsQueryable();

        private IEnumerable<TqRegistrationPathway> CreateRegistrationPathways() =>
        new List<TqRegistrationPathway>
            {
                new TqRegistrationPathway
                {
                    Id = 1,
                    TqRegistrationProfileId = 1,
                    TqProviderId = 1,
                    AcademicYear = 2021,
                    StartDate = new DateTime(2022, 9, 1),
                    EndDate = null,
                    Status = RegistrationPathwayStatus.Active,
                    IsBulkUpload = false,
                    IsPendingWithdrawal = false,
                    IndustryPlacements = new List<IndustryPlacement>(),
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        Id = 1,
                        UniqueLearnerNumber = 3333333333,
                        Firstname = "John",
                        Lastname = "Doe"
                    },
                    TqProvider = new TqProvider
                    {
                        Id = 1,
                        TlProviderId = 1,
                        TlProvider = new TlProvider
                        {
                            Id = 1,
                            UkPrn = 10000001,
                            Name = "Test Provider 1"
                        }
                    }
                },
                new TqRegistrationPathway
                {
                    Id = 2,
                    TqRegistrationProfileId = 2,
                    TqProviderId = 1,
                    AcademicYear = 2021,
                    StartDate = new DateTime(2022, 9, 1),
                    EndDate = null,
                    Status = RegistrationPathwayStatus.Active,
                    IsBulkUpload = false,
                    IsPendingWithdrawal = false,
                    IndustryPlacements = CreateIndustryPlacements().Where(e=> e.Id == 1).ToList() ,
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        Id = 2,
                        UniqueLearnerNumber = 4444444444,
                        Firstname = "Tom",
                        Lastname = "Cruise"
                    },
                    TqProvider = new TqProvider
                    {
                        Id = 1,
                        TlProviderId = 1,
                        TlProvider = new TlProvider
                        {
                            Id = 1,
                            UkPrn = 10000001,
                            Name = "Test Provider 1"
                        }
                    }
                },
                new TqRegistrationPathway
                {
                    Id = 3,
                    TqRegistrationProfileId = 1,
                    TqProviderId = 1,
                    AcademicYear = 2021,
                    StartDate = new DateTime(2022, 9, 1),
                    EndDate = null,
                    Status = RegistrationPathwayStatus.Withdrawn,
                    IsBulkUpload = false,
                    IsPendingWithdrawal = false,
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        Id = 3,
                        UniqueLearnerNumber = 1234567890,
                        Firstname = "John",
                        Lastname = "Doe"
                    },
                    TqProvider = new TqProvider
                    {
                        Id = 2,
                        TlProviderId = 2,
                        TlProvider = new TlProvider
                        {
                            Id = 2,
                            UkPrn = 10000002,
                            Name = "Test Provider 2"
                        }
                    }
                }
            }.AsQueryable();

        private IEnumerable<IndustryPlacement> CreateIndustryPlacements() =>
            new List<IndustryPlacement>
            {
                new IndustryPlacement
                {
                    Id = 1,
                    TqRegistrationPathwayId = 1,
                    Status = IndustryPlacementStatus.Completed
                },
                new IndustryPlacement
                {
                    Id = 2,
                    TqRegistrationPathwayId = 2,
                    Status = IndustryPlacementStatus.WillNotComplete
                }
            }.AsQueryable();
    }

}
