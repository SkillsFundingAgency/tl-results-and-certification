using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_DownloadOverallResultsData_With_Dual_Speciality_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private const int Uln = 1111111111;
        private const EnumAwardingOrganisation AwardingOrganisation = EnumAwardingOrganisation.Pearson;
        private const string PreviousAssessmentName = "Previous Assessment";

        private readonly DateTime _publishDate = DateTime.Today.AddDays(-1);

        private TqRegistrationProfile _registration;
        private TlDualSpecialism _dualSpecialism;

        private IList<DownloadOverallResultsData> _actualResult;

        public override void Given()
        {
            // Registration seed
            var tlPathway = TlevelDataProvider.CreateTlPathway(DbContext, AwardingOrganisation);
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, AwardingOrganisation);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, tlPathway, tlAwardingOrganisation);

            var tlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            var tqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider);

            var profile = new TqRegistrationProfileBuilder().Build(Uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider);

            _registration = tqRegistrationProfile;

            // Specialisms seed
            var heatingEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202101", "Heating Engineering");
            var plumbingEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202102", "Plumbing Engineering");
            var specialisms = new[] { heatingEngineeringSpecialism, plumbingEngineeringSpecialism };
            DbContext.AddRange(specialisms);

            // Dual specialism seed
            _dualSpecialism = CreateDualSpecialism(tlPathway.Id, "ZTLOS030", "Plumbing and Heating Engineering");
            DbContext.Add(_dualSpecialism);

            DbContext.SaveChanges();

            // Dual specialism to specialism seed
            var dualSpecialismToSpecialism = CreateDualSpecialismToSpecialism(_dualSpecialism, heatingEngineeringSpecialism, plumbingEngineeringSpecialism);
            DbContext.AddRange(dualSpecialismToSpecialism);

            // Registration specialisms seed
            IEnumerable<TqRegistrationSpecialism> registrationSpecialisms = specialisms.Select(s => new TqRegistrationSpecialism
            {
                TlSpecialismId = s.Id,
                TqRegistrationPathwayId = tqRegistrationPathway.Id
            });

            DbContext.AddRange(registrationSpecialisms);

            // Overall results seed
            var overallResult = CreateOverallResult(tqRegistrationPathway.Id, "Distinction", "Merit", tlPathway, specialisms);
            OverallResultDataProvider.CreateOverallResult(DbContext, new[] { overallResult });

            // SeedPreviousAssessment
            AssessmentSeriesDataProvider.CreateAssessmentSeries(DbContext);

            var prevAssessmentSeries = new AssessmentSeries { Name = PreviousAssessmentName, Year = 2020, StartDate = DateTime.Today.AddMonths(-3), EndDate = DateTime.Today.AddDays(-2), ResultPublishDate = _publishDate, ComponentType = ComponentType.Core, ResultCalculationYear = 2020 };
            AssessmentSeriesDataProvider.CreateAssessmentSeries(DbContext, prevAssessmentSeries, true);

            DbContext.SaveChanges();
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(Provider provider)
        {
            var prevAssessment = DbContext.AssessmentSeries.FirstOrDefault(x => x.Name.Equals(PreviousAssessmentName, StringComparison.InvariantCultureIgnoreCase));
            prevAssessment.ResultPublishDate = _publishDate;
            DbContext.SaveChanges();

            _actualResult = await OverallResultCalculationService.DownloadOverallResultsDataAsync((long)provider);
        }

        [Theory]
        [InlineData(Provider.BarsleyCollege, Uln)]
        public async Task Then_Expected_Results_Are_Returned(Provider provider, int uln)
        {
            await WhenAsync(provider);

            _actualResult.Should().HaveCount(1);

            var actualResult = _actualResult.FirstOrDefault(x => x.Uln == uln);
            var expectedRegistration = _registration;

            actualResult.Should().NotBeNull();
            actualResult.Uln.Should().Be(expectedRegistration.UniqueLearnerNumber);
            actualResult.LastName.Should().Be(expectedRegistration.Lastname);
            actualResult.FirstName.Should().Be(expectedRegistration.Firstname);
            actualResult.DisplayDateOfBirth.Should().Be(expectedRegistration.DateofBirth.ToString("dd-MMM-yyyy"));
            actualResult.DisplayStartYear.Should().Be($"{expectedRegistration.TqRegistrationPathways.FirstOrDefault().AcademicYear} to {expectedRegistration.TqRegistrationPathways.FirstOrDefault().AcademicYear + 1}");

            var expectedOverallResults = expectedRegistration.TqRegistrationPathways.FirstOrDefault().OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            var expectedOverallResultsDetails = JsonConvert.DeserializeObject<OverallResultDetail>(expectedOverallResults.Details);
            actualResult.Tlevel.Should().Be($"\"{expectedOverallResultsDetails.TlevelTitle}\"");
            actualResult.CoreCode.Should().Be($"{expectedOverallResultsDetails.PathwayLarId}");
            actualResult.CoreComponent.Should().Be($"\"{expectedOverallResultsDetails.PathwayName}\"");
            actualResult.CoreResult.Should().Be($"{expectedOverallResultsDetails.PathwayResult}");

            actualResult.SpecialismCode.Should().Be(_dualSpecialism.LarId);
            actualResult.SpecialismComponent.Should().Be($"\"{_dualSpecialism.Name}\"");
            actualResult.SpecialismResult.Should().Be(expectedOverallResults.SpecialismResultAwarded);

            actualResult.IndustryPlacementStatus.Should().Be($"{expectedOverallResultsDetails.IndustryPlacementStatus}");
            actualResult.OverallResult.Should().Be(expectedOverallResults.ResultAwarded);
        }

        private TlSpecialism CreateSpecialism(int pathwayId, string larId, string name)
        {
            return new TlSpecialism
            {
                TlPathwayId = pathwayId,
                LarId = larId,
                Name = name
            };
        }

        private TlDualSpecialism CreateDualSpecialism(int pathwayId, string larId, string name)
        {
            return new TlDualSpecialism
            {
                TlPathwayId = pathwayId,
                LarId = larId,
                Name = name
            };
        }

        private IEnumerable<TlDualSpecialismToSpecialism> CreateDualSpecialismToSpecialism(TlDualSpecialism dualSpecialism, TlSpecialism firstSpecialism, TlSpecialism secondSpecialism)
        {
            return new[]
            {
                new TlDualSpecialismToSpecialism
                {
                    TlDualSpecialismId = dualSpecialism.Id,
                    TlSpecialismId = firstSpecialism.Id
                },
                new TlDualSpecialismToSpecialism
                {
                    TlDualSpecialismId = dualSpecialism.Id,
                    TlSpecialismId = secondSpecialism.Id
                }
            };
        }

        private OverallResult CreateOverallResult(
            int registrationPathwayId, 
            string resultAwarded, 
            string specialismResultAwarded,
            TlPathway pathway,
            IEnumerable<TlSpecialism> specialisms)
        {
            var details = new
            {
                IndustryPlacementStatus = "Completed",
                OverallResult = resultAwarded,
                PathwayLarId = pathway.LarId,
                PathwayName = pathway.Name,
                PathwayResult = "A*",
                SpecialismDetails = specialisms.Select(s => new
                {
                    SpecialismLarId = s.LarId,
                    SpecialismName = s.Name,
                    SpecialismResult = specialismResultAwarded
                }),
                pathway.TlevelTitle
            };

            var printAvailableFrom = DateTime.Now.Date.AddMonths(5).AddDays(1);

            return new OverallResult
            {
                TqRegistrationPathwayId = registrationPathwayId,
                Details = JsonConvert.SerializeObject(details),
                ResultAwarded = resultAwarded,
                SpecialismResultAwarded = specialismResultAwarded,
                CalculationStatus = CalculationStatus.Completed,
                IsOptedin = true,
                CertificateType = PrintCertificateType.Certificate,
                PrintAvailableFrom = printAvailableFrom,
                PublishDate = _publishDate
            };
        }
    }
}