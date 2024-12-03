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

            DbContext.SaveChanges();

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

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            ReviewChangeStartYearRequest request = new()
            {
                RegistrationPathwayId = 1,
                ChangeReason = "Test Reason",
                ContactName = "Test User",
                ChangeStartYearDetails = new ChangeStartYearDetails { StartYearFrom = 2020, StartYearTo = 2021 },
                RequestDate = DateTime.Now,
                ZendeskId = "1234567890",
                CreatedBy = "System"
            };

            await WhenAsync(request);

            TqRegistrationPathway expected = _registrations.SingleOrDefault(x => x.UniqueLearnerNumber == 1111111111).TqRegistrationPathways.FirstOrDefault();
            TqRegistrationPathway actual = DbContext.TqRegistrationPathway.FirstOrDefault(ip => ip.Id == request.RegistrationPathwayId);

            // Assert
            actual.AcademicYear.Should().Be(expected.AcademicYear);
            actual.ModifiedBy.Should().Be(request.CreatedBy);
            actual.ModifiedOn.Should().HaveValue();
        }
    }
}