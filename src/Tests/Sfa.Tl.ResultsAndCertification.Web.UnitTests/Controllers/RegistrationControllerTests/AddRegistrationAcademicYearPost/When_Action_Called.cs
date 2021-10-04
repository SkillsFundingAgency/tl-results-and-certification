using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class Then_On_Success_With_Selected_Specialisms_Redirected_To_AddRegistrationCheckAndSubmit_Route : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectSpecialismViewModel _selectSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private IList<AcademicYear> _academicYears;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _selectedAcademicYear = "2020".ToString();
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } } };
            _selectSpecialismViewModel = new SelectSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            cacheResult = new RegistrationViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectSpecialisms = _selectSpecialismViewModel
            };

            SelectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };

            RegistrationLoader.GetCurrentAcademicYearsAsync().Returns(_academicYears);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
