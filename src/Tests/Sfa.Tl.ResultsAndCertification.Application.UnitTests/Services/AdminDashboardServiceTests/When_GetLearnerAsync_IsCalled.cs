using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public class When_GetLearnerAsync_IsCalled : AdminDashboardServiceBaseTest
    {
        private TqRegistrationPathway _repoResult;
        private AdminLearnerRecord _actualResult;
        private const int RegistrationPathwayId = 1;

        public override void Given()
        {
            _repoResult = new TqRegistrationPathway
            {
                Id = 1,
                TqRegistrationProfile = new TqRegistrationProfile
                {
                    Firstname = "John",
                    Lastname = "Smith",
                    UniqueLearnerNumber = 1111111111,
                    DateofBirth = new DateTime(2005, 1, 23),
                    MathsStatus = SubjectStatus.Achieved,
                    EnglishStatus = SubjectStatus.Achieved,
                    TqRegistrationPathways = new[]
                    {
                        new TqRegistrationPathway
                        {
                            Id = 125,
                            TqProvider = new TqProvider
                            {
                                TqAwardingOrganisation = new TqAwardingOrganisation
                                {
                                    TlPathway = new TlPathway
                                    {
                                        LarId = "6100008X",
                                        Name = "Finance",
                                        TlevelTitle = "T Level in Finance",
                                        StartYear = 2021
                                    }
                                }
                            },
                            AcademicYear = 2023,
                            Status = RegistrationPathwayStatus.Active
                        }
                    }
                }
            };

            AdminDashboardRepository.GetLearnerRecordAsync(RegistrationPathwayId).Returns(_repoResult);
        }

        public override async Task When()
        {
            _actualResult = await AdminDashboardService.GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
        }
    }
}
