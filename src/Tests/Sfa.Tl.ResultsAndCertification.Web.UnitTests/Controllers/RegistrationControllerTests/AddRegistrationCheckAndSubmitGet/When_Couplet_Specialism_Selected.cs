using FluentAssertions;
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
    public class When_Couplet_Specialism_Selected : TestSetup
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
                Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321|7654322", DisplayName = "Plubming (7654321) and Heating (7654322)", IsSelected = true } },
                SpecialismsLookup = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("7654321", "Plubming"), new KeyValuePair<string, string>("7654322", "Heating"), new KeyValuePair<string, string>("7654333", "Test Childcare") }
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
        public void Then_Expected_SummarySpecialisms_Is_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(CheckAndSubmitViewModel));

            var model = viewResult.Model as CheckAndSubmitViewModel;
            model.Should().NotBeNull();

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(CheckAndSubmitContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().NotBeNull();
            model.SummarySpecialisms.Value.Count().Should().Be(2);
            model.SummarySpecialisms.Value.First().Should().Be("Plubming (7654321)");
            model.SummarySpecialisms.Value.Last().Should().Be("Heating (7654322)");
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
            model.SummarySpecialisms.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummarySpecialisms.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
