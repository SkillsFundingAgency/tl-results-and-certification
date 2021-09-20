using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class When_Success : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectAcademicYearViewModel _selectAcademicYearViewModel;
        private IList<AcademicYear> _academicYears;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _selectedAcademicYear = _academicYears.FirstOrDefault().Year.ToString();
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _selectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
            cacheResult = new RegistrationViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectAcademicYear = _selectAcademicYearViewModel
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
