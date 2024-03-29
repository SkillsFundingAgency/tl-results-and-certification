﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCheckAndSubmitGet
{
    public class When_Cache_Found : TestSetup
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
        private IList<AcademicYear> _academicYears;
        private string _coreCode = "12345678";
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            _routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _ulnViewModel =  new UlnViewModel { Uln = "1234567890" };
            _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
            _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel 
            { 
                PathwayCode = _coreCode, 
                PathwayName = "Education", 
                Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } },
                SpecialismsLookup = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("7654321", "Test Education"), new KeyValuePair<string, string>("7654322", "Test Design"), new KeyValuePair<string, string>("7654333", "Test Childcare") }
            };
            _selectSpecialismViewModel = new SelectSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            _academicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = "2020", AcademicYears = _academicYears };
            
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

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(CheckAndSubmitViewModel));

            var model = viewResult.Model as CheckAndSubmitViewModel;
            model.Should().NotBeNull();

            // Summary ULN
            model.SummaryUln.Should().NotBeNull();
            model.SummaryUln.Title.Should().Be(CheckAndSubmitContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_ulnViewModel.Uln);
            model.SummaryUln.RouteName.Should().Be(RouteConstants.AddRegistrationUln);
            model.SummaryUln.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary LearnerName
            model.SummaryLearnerName.Should().NotBeNull();
            model.SummaryLearnerName.Title.Should().Be(CheckAndSubmitContent.Title_Name_Text);
            model.SummaryLearnerName.Value.Should().Be($"{_learnersNameViewModel.Firstname} {_learnersNameViewModel.Lastname}");
            model.SummaryLearnerName.RouteName.Should().Be(RouteConstants.AddRegistrationLearnersName);
            model.SummaryLearnerName.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryLearnerName.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary DateofBirth
            model.SummaryDateofBirth.Should().NotBeNull();
            model.SummaryDateofBirth.Title.Should().Be(CheckAndSubmitContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be($"{_dateofBirthViewModel.Day}/{_dateofBirthViewModel.Month}/{_dateofBirthViewModel.Year}");
            model.SummaryDateofBirth.RouteName.Should().Be(RouteConstants.AddRegistrationDateofBirth);
            model.SummaryDateofBirth.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryDateofBirth.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(CheckAndSubmitContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_selectProviderViewModel.SelectedProviderDisplayName);
            model.SummaryProvider.RouteName.Should().Be(RouteConstants.AddRegistrationProvider);
            model.SummaryProvider.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryProvider.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Core
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(CheckAndSubmitContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(_selectCoreViewModel.SelectedCoreDisplayName);
            model.SummaryCore.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
            model.SummaryCore.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryCore.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(CheckAndSubmitContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeEquivalentTo(_selectSpecialismViewModel.PathwaySpecialisms.Specialisms.Where(s => s.IsSelected).Select(s => s.DisplayName));
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
            model.SummarySpecialisms.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummarySpecialisms.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Academic Year
            model.SummaryAcademicYear.Should().NotBeNull();
            model.SummaryAcademicYear.Title.Should().Be(CheckAndSubmitContent.Title_AcademicYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(_academicYearViewModel.AcademicYears.FirstOrDefault().Name);
            model.SummaryAcademicYear.RouteName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
            model.SummaryAcademicYear.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryAcademicYear.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
        }
    }
}
