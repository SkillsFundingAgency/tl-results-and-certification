using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearPost
{
    public class Then_On_Success_Redirected_To_AddRegistrationCheckAndSubmit_Route : When_AddRegistrationAcademicYear_Action_Is_Called
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
        public void Then_On_Success_Redirected_To_AddRegistration_CheckAndSubmit_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
