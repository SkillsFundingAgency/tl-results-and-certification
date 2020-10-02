using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearGet
{
    public class When_AcademicYear_Cache_Found : TestSetup
    {
        private ReregisterViewModel cacheResult;

        private ReregisterSpecialismQuestionViewModel _specialismQuestionViewModel;
        private ReregisterAcademicYearViewModel _reregisterAcademicYearViewModel;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = ((int)AcademicYear.Year2020).ToString();
            _specialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _reregisterAcademicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
            
            cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                ReregisterSpecialisms = null,
                ReregisterAcademicYear = _reregisterAcademicYearViewModel
            };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);

            RegistrationLoader
                .GetRegistrationDetailsAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Withdrawn)
                .Returns(new RegistrationDetailsViewModel { Status = RegistrationPathwayStatus.Withdrawn });
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterAcademicYearViewModel));

            var model = viewResult.Model as ReregisterAcademicYearViewModel;
            model.Should().NotBeNull();

            model.SelectedAcademicYear.Should().Be(_selectedAcademicYear);
            model.IsValidAcademicYear.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
        }
    }
}
