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
        private const long FirstUln = 1111111111;
        private const long SecondUln = 1111111112;
        private const long ThirdUln = 1111111113;

        private const string PlumbingAndHeatingEngineeringDualSpecialismLarId = "ZTLOS030";
        private const string HeatingEngineeringAndVentilationDualSpecialismLarId = "ZTLOS031";
        private const string RefrigerationAndAirConditioningEngineeringDualSpecialismLarId = "ZTLOS032";

        private readonly Dictionary<long, string> _ulnDualSpecialismDict = new()
        {
            [FirstUln] = PlumbingAndHeatingEngineeringDualSpecialismLarId,
            [SecondUln] = HeatingEngineeringAndVentilationDualSpecialismLarId,
            [ThirdUln] = RefrigerationAndAirConditioningEngineeringDualSpecialismLarId
        };

        private const EnumAwardingOrganisation AwardingOrganisation = EnumAwardingOrganisation.Pearson;
        private const string PreviousAssessmentName = "Previous Assessment";

        private readonly DateTime _publishDate = DateTime.Today.AddDays(-1);

        private readonly List<TqRegistrationProfile> _tqRegistrationProfiles = new();
        private readonly List<TqRegistrationPathway> _tqRegistrationPathways = new();
        private TlDualSpecialism[] _dualSpecialisms;

        private IList<DownloadOverallResultsData> _actualResult;

        public override void Given()
        {
            // Registrations seed
            var tlPathway = TlevelDataProvider.CreateTlPathway(DbContext, AwardingOrganisation);
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, AwardingOrganisation);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, tlPathway, tlAwardingOrganisation);

            var tlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            var tqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider);

            foreach (var currentUlnDualSpecialism in _ulnDualSpecialismDict)
            {
                var profile = new TqRegistrationProfileBuilder().Build(currentUlnDualSpecialism.Key);
                var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
                var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider);

                _tqRegistrationProfiles.Add(tqRegistrationProfile);
                _tqRegistrationPathways.Add(tqRegistrationPathway);
            }

            // Specialisms seed
            var heatingEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202101", "Heating Engineering");
            var plumbingEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202102", "Plumbing Engineering");
            var ventilationSpecialism = CreateSpecialism(tlPathway.Id, "10202105", "Ventilation");
            var refrigerationEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202103", "Refrigeration Engineering");
            var airConditioningEngineeringSpecialism = CreateSpecialism(tlPathway.Id, "10202104", "Air Conditioning Engineering");

            var specialisms = new[] { heatingEngineeringSpecialism, plumbingEngineeringSpecialism, ventilationSpecialism, refrigerationEngineeringSpecialism, airConditioningEngineeringSpecialism };
            DbContext.AddRange(specialisms);

            // Dual specialism seed
            var plumbingAndHeatingEngineeringDualSpecialism = CreateDualSpecialism(tlPathway.Id, PlumbingAndHeatingEngineeringDualSpecialismLarId, "Plumbing and Heating Engineering");
            var heatingEngineeringAndVentilationDualSpecialism = CreateDualSpecialism(tlPathway.Id, HeatingEngineeringAndVentilationDualSpecialismLarId, "Heating Engineering and Ventilation");
            var refrigerationAndAirConditioningEngineeringDualSpecialism = CreateDualSpecialism(tlPathway.Id, RefrigerationAndAirConditioningEngineeringDualSpecialismLarId, "Refrigeration Engineering and Air Conditioning Engineering");

            _dualSpecialisms = new[] { plumbingAndHeatingEngineeringDualSpecialism, heatingEngineeringAndVentilationDualSpecialism, refrigerationAndAirConditioningEngineeringDualSpecialism };
            DbContext.AddRange(_dualSpecialisms);

            DbContext.SaveChanges();

            // Dual specialism to specialism seed
            var plumbingAndHeatingEngineeringDualSpecialismToSpecialism = CreateDualSpecialismToSpecialism(plumbingAndHeatingEngineeringDualSpecialism, heatingEngineeringSpecialism, plumbingEngineeringSpecialism);
            var heatingEngineeringAndVentilationDualSpecialismToSpecialism = CreateDualSpecialismToSpecialism(heatingEngineeringAndVentilationDualSpecialism, heatingEngineeringSpecialism, ventilationSpecialism);
            var refrigerationAndAirConditioningEngineeringDualSpecialismToSpecialism = CreateDualSpecialismToSpecialism(refrigerationAndAirConditioningEngineeringDualSpecialism, refrigerationEngineeringSpecialism, airConditioningEngineeringSpecialism);

            DbContext.AddRange(plumbingAndHeatingEngineeringDualSpecialismToSpecialism);
            DbContext.AddRange(heatingEngineeringAndVentilationDualSpecialismToSpecialism);
            DbContext.AddRange(refrigerationAndAirConditioningEngineeringDualSpecialismToSpecialism);

            // Registration specialisms seed
            TqRegistrationPathway firstRegistrationPathway = _tqRegistrationPathways.First(p => p.TqRegistrationProfile.UniqueLearnerNumber == FirstUln);
            TqRegistrationPathway secondRegistrationPathway = _tqRegistrationPathways.First(p => p.TqRegistrationProfile.UniqueLearnerNumber == SecondUln);
            TqRegistrationPathway thirdRegistrationPathway = _tqRegistrationPathways.First(p => p.TqRegistrationProfile.UniqueLearnerNumber == ThirdUln);

            IEnumerable<TqRegistrationSpecialism> firstRegistrationSpecialisms = CreateRegistrationSpecialisms(plumbingAndHeatingEngineeringDualSpecialismToSpecialism, firstRegistrationPathway);
            IEnumerable<TqRegistrationSpecialism> secondRegistrationSpecialisms = CreateRegistrationSpecialisms(heatingEngineeringAndVentilationDualSpecialismToSpecialism, secondRegistrationPathway);
            IEnumerable<TqRegistrationSpecialism> thirdRegistrationSpecialisms = CreateRegistrationSpecialisms(refrigerationAndAirConditioningEngineeringDualSpecialismToSpecialism, thirdRegistrationPathway);

            DbContext.AddRange(firstRegistrationSpecialisms);
            DbContext.AddRange(secondRegistrationSpecialisms);
            DbContext.AddRange(thirdRegistrationSpecialisms);

            // Overall results seed
            var firstOverallResult = CreateOverallResult(firstRegistrationPathway.Id, "Distinction", "Distinction", tlPathway, new[] { heatingEngineeringSpecialism, plumbingEngineeringSpecialism });
            var secondOverallResult = CreateOverallResult(secondRegistrationPathway.Id, "Distinction", "Merit", tlPathway, new[] { heatingEngineeringSpecialism, ventilationSpecialism });
            var thirdOverallResult = CreateOverallResult(thirdRegistrationPathway.Id, "Pass", "Pass", tlPathway, new[] { refrigerationEngineeringSpecialism, airConditioningEngineeringSpecialism });

            OverallResultDataProvider.CreateOverallResult(DbContext, new[] { firstOverallResult, secondOverallResult, thirdOverallResult });

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
        [InlineData(Provider.BarsleyCollege, FirstUln)]
        [InlineData(Provider.BarsleyCollege, SecondUln)]
        [InlineData(Provider.BarsleyCollege, ThirdUln)]
        public async Task Then_Expected_Results_Are_Returned(Provider provider, int uln)
        {
            await WhenAsync(provider);

            _actualResult.Where(x => x.Uln == uln).Should().HaveCount(1);

            var actualResult = _actualResult.FirstOrDefault(x => x.Uln == uln);
            var expectedRegistration = _tqRegistrationProfiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);

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

            actualResult.SpecialismCode.Should().Be(_ulnDualSpecialismDict[uln]);
            actualResult.SpecialismComponent.Should().Be($"\"{_dualSpecialisms.FirstOrDefault(d => d.LarId == _ulnDualSpecialismDict[uln]).Name}\"");
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

        private IEnumerable<TqRegistrationSpecialism> CreateRegistrationSpecialisms(IEnumerable<TlDualSpecialismToSpecialism> dualSpecialismToSpecialisms, TqRegistrationPathway tqRegistrationPathway)
        {
            return dualSpecialismToSpecialisms.Select(p => new TqRegistrationSpecialism
            {
                TlSpecialismId = p.TlSpecialismId,
                TqRegistrationPathwayId = tqRegistrationPathway.Id
            });
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