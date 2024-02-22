using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public class When_ProcessReviewChangeStartYear_IsCalled : AdminDashboardServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            var currentYearUln = new List<long> { 1111111111 };
            RegisterUlnForNextAcademicYear(_registrations, currentYearUln);

            DbContext.SaveChanges();
            DetachAll();

            CreateAdminDasboardService();
        }

        private bool _actualResult;

        public override Task When()
        {
            return Task.CompletedTask;
        }
        public async Task WhenAsync(ReviewChangeStartYearRequest request)
        {
            _actualResult = await AdminDashboardService.ProcessChangeStartYearAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(ReviewChangeStartYearRequest request, bool expectedResponse, long uln)
        {
            await WhenAsync(request);

            if (expectedResponse == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            var expectedRegistrationPathway = _registrations.SingleOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();

            expectedRegistrationPathway.Should().NotBeNull();

            var actualIndustryPlacement = DbContext.TqRegistrationPathway.FirstOrDefault(ip => ip.Id == request.RegistrationPathwayId);

            // Assert
            request.RegistrationPathwayId.Should().Be(actualIndustryPlacement.Id);
            request.ChangeStartYearDetails.StartYearTo.Should().Be(actualIndustryPlacement.AcademicYear);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Uln not found
                    new object[]
                    {
                        new ReviewChangeStartYearRequest
                        {
                            ChangeReason = "Test Reason",
                            ContactName = "Test User",
                            RegistrationPathwayId = 1,
                            ChangeStartYearDetails = new ChangeStartYearDetails() { StartYearFrom = 2022, StartYearTo = 2021 },
                            RequestDate = DateTime.Now,
                            ZendeskId = "1234567890",
                            CreatedBy = "System"
                        },
                        true,
                        1111111111
                    }
                };
            }
        }
    }
}