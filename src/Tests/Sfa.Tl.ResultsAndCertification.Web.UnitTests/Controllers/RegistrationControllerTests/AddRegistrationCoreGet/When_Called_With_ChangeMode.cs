﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCoreGet
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;
        private LearnersNameViewModel _learnersNameViewModel;
        private DateofBirthViewModel _dateofBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectSpecialismViewModel _selectSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private SelectAcademicYearViewModel _academicYearViewModel;
        private string _coreCode = "12345678";
        private long _providerUkprn = 987654321;

        public override void Given()
        {
            IsChangeMode = true;
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };
            _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
            _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = _providerUkprn.ToString(), SelectedProviderDisplayName = "Barnsley College (98765432)", ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Barnsley College (98765432)", Value = _providerUkprn.ToString() } } };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
            _selectSpecialismViewModel = new SelectSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            _academicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = "2020" };

            cacheResult = new RegistrationViewModel
            {
                Uln = _ulnViewModel,
                LearnersName = _learnersNameViewModel,
                DateofBirth = _dateofBirthViewModel,
                SelectProvider = _selectProviderViewModel,
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectSpecialisms = _selectSpecialismViewModel,
                SelectAcademicYear = _academicYearViewModel
            };

            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(Ukprn, _providerUkprn).Returns(_selectCoreViewModel);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectCoreViewModel));

            var model = viewResult.Model as SelectCoreViewModel;
            model.Should().NotBeNull();

            model.SelectedCoreCode.Should().Be(_selectCoreViewModel.SelectedCoreCode);
            model.CoreSelectList.Should().NotBeNull();
            model.CoreSelectList.Count.Should().Be(_selectCoreViewModel.CoreSelectList.Count);
            model.IsChangeMode.Should().Be(IsChangeMode);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
