using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class When_Success : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectAcademicYearViewModel _selectAcademicYearViewModel;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = ((int)AcademicYear.Year2020).ToString();
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _selectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
            cacheResult = new RegistrationViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectAcademicYear = _selectAcademicYearViewModel
            };
            SelectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear };
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
