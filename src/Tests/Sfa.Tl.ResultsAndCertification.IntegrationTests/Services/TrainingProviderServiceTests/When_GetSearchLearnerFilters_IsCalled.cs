using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_GetSearchLearnerFilters_IsCalled : TrainingProviderServiceBaseTest
    {
        private SearchLearnerFilters _actualResult;
        private IList<FilterLookupData> _mockAcademicYears;
        private IList<FilterLookupData> _mockTlevels;
        private IList<FilterLookupData> _expecetedStatusFilters;

        public override void Given()
        {
            // Create Service
            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);
            TrainingProviderRepositoryLogger = new Logger<TrainingProviderRepository>(new NullLoggerFactory());
            TrainingProviderRepository = Substitute.For<ITrainingProviderRepository>();
            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());
            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, TrainingProviderRepository, TrainingProviderServiceLogger);

            // Mock data 
            _mockAcademicYears = new List<FilterLookupData> { new FilterLookupData { Id = 2021, Name = "2021 to 2022", IsSelected = false }, new FilterLookupData { Id = 2022, Name = "2022 to 2023", IsSelected = false } };
            TrainingProviderRepository.GetSearchAcademicYearFiltersAsync(Arg.Any<DateTime>()).Returns(_mockAcademicYears);

            _mockTlevels = new List<FilterLookupData> { new FilterLookupData { Id = 1, Name = "Design, Survey and Planning", IsSelected = false }, new FilterLookupData { Id = 2, Name = "Health", IsSelected = false } };
            TrainingProviderRepository.GetSearchTlevelFiltersAsync().Returns(_mockTlevels);

            _expecetedStatusFilters = new List<FilterLookupData>
            {
                new FilterLookupData { Id = 1, Name = "English level", IsSelected = false },
                new FilterLookupData { Id = 2, Name = "Maths level", IsSelected = false },
                new FilterLookupData { Id = 3, Name = "Industry placement", IsSelected = false },
                new FilterLookupData { Id = 4, Name = "All incomplete records", IsSelected = false }
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderService.GetSearchLearnerFiltersAsync((int)Provider.BarnsleyCollege);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();

            _actualResult.Should().NotBeNull();
            _actualResult.AcademicYears.Should().BeEquivalentTo(_mockAcademicYears);
            _actualResult.Tlevels.Should().BeEquivalentTo(_mockTlevels);
            _actualResult.Status.Should().BeEquivalentTo(_expecetedStatusFilters);
        }
    }
}
