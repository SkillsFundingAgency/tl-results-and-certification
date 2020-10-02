using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearGet
{
    public class When_AcademicYear_Cache_NotFound : TestSetup
    {
        private ReregisterViewModel cacheResult;

        private ReregisterSpecialismQuestionViewModel _specialismQuestionViewModel;
        private ReregisterSpecialismViewModel _selectSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;

        public override void Given()
        {
            _specialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } } };
            _selectSpecialismViewModel = new ReregisterSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            
            cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                ReregisterSpecialisms = _selectSpecialismViewModel,
                ReregisterAcademicYear = null
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

            model.SelectedAcademicYear.Should().BeNull();
            model.IsValidAcademicYear.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterSpecialisms);
        }
    }
}
