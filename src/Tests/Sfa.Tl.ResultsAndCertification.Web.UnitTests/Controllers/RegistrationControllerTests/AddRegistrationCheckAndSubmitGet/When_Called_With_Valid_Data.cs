using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCheckAndSubmitGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;
        private LearnersNameViewModel _learnersNameViewModel;
        private DateofBirthViewModel _dateofBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectAcademicYearViewModel _academicYearViewModel;
        private string _coreCode = "12345678";
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            _routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };
            _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
            _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _academicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = "2020" };

            cacheResult = new RegistrationViewModel
            {
                Uln = _ulnViewModel,
                LearnersName = _learnersNameViewModel,
                DateofBirth = _dateofBirthViewModel,
                SelectProvider = _selectProviderViewModel,
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel,
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
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary DateofBirth
            model.SummaryDateofBirth.Should().NotBeNull();
            model.SummaryDateofBirth.Title.Should().Be(CheckAndSubmitContent.Title_DateofBirth_Text);
            model.SummaryDateofBirth.Value.Should().Be($"{_dateofBirthViewModel.Day}/{_dateofBirthViewModel.Month}/{_dateofBirthViewModel.Year}");
            model.SummaryDateofBirth.RouteName.Should().Be(RouteConstants.AddRegistrationDateofBirth);
            model.SummaryDateofBirth.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(CheckAndSubmitContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_selectProviderViewModel.SelectedProviderDisplayName);
            model.SummaryProvider.RouteName.Should().Be(RouteConstants.AddRegistrationProvider);
            model.SummaryProvider.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Core
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(CheckAndSubmitContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(_selectCoreViewModel.SelectedCoreDisplayName);
            model.SummaryCore.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
            model.SummaryCore.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(CheckAndSubmitContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeNullOrEmpty();
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialisms);
            model.SummarySpecialisms.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Academic Year
            model.SummaryAcademicYear.Should().NotBeNull();
            model.SummaryAcademicYear.Title.Should().Be(CheckAndSubmitContent.Title_AcademicYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(EnumExtensions.GetDisplayName<AcademicYear>(_academicYearViewModel.SelectedAcademicYear));
            model.SummaryAcademicYear.RouteName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
            model.SummaryAcademicYear.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryUln.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
        }
    }
}
