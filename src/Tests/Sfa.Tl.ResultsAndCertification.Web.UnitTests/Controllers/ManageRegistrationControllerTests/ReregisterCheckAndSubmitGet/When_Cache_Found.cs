using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitGet
{
    public class When_Cache_Found : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private ReregisterSpecialismViewModel _reregisterSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private ReregisterAcademicYearViewModel _academicYearViewModel;
        private readonly string _coreCode = "12345678";
        private readonly long _uln = 1234567890;
        private Dictionary<string, string> _routeAttributes;
        private RegistrationDetailsViewModel _registrationDetails = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;
        private IList<AcademicYear> _academicYears;

        public override void Given()
        {
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            _reregisterProviderViewModel = new ReregisterProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
            _reregisterSpecialismViewModel = new ReregisterSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            _academicYearViewModel = new ReregisterAcademicYearViewModel { ProfileId = ProfileId, SelectedAcademicYear = "2020", AcademicYears = _academicYears };

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel,
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel,
                ReregisterSpecialisms = _reregisterSpecialismViewModel,
                ReregisterAcademicYear = _academicYearViewModel
            };

            _registrationDetails = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Uln = _uln,
                Status = _registrationPathwayStatus
            };

            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(_registrationDetails);
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterCheckAndSubmitViewModel));

            var model = viewResult.Model as ReregisterCheckAndSubmitViewModel;
            model.Should().NotBeNull();

            // Summary ULN
            model.SummaryUln.Title.Should().Be(CheckAndSubmitContent.Title_Uln_Text);
            model.SummaryUln.Value.Should().Be(_uln.ToString());

            // Summary Provider
            model.SummaryProvider.Should().NotBeNull();
            model.SummaryProvider.Title.Should().Be(CheckAndSubmitContent.Title_Provider_Text);
            model.SummaryProvider.Value.Should().Be(_reregisterProviderViewModel.SelectedProviderDisplayName);
            model.SummaryProvider.RouteName.Should().Be(RouteConstants.ReregisterProvider);
            model.SummaryProvider.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryProvider.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Core
            model.SummaryCore.Should().NotBeNull();
            model.SummaryCore.Title.Should().Be(CheckAndSubmitContent.Title_Core_Text);
            model.SummaryCore.Value.Should().Be(_reregisterCoreViewModel.SelectedCoreDisplayName);
            model.SummaryCore.RouteName.Should().Be(RouteConstants.ReregisterCore);
            model.SummaryCore.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryProvider.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Specialisms
            model.SummarySpecialisms.Should().NotBeNull();
            model.SummarySpecialisms.Title.Should().Be(CheckAndSubmitContent.Title_Specialism_Text);
            model.SummarySpecialisms.Value.Should().BeEquivalentTo(_reregisterSpecialismViewModel.PathwaySpecialisms.Specialisms.Where(s => s.IsSelected).Select(s => s.DisplayName));
            model.SummarySpecialisms.RouteName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
            model.SummarySpecialisms.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryProvider.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            // Summary Academic Year
            model.SummaryAcademicYear.Should().NotBeNull();
            model.SummaryAcademicYear.Title.Should().Be(CheckAndSubmitContent.Title_AcademicYear_Text);
            model.SummaryAcademicYear.Value.Should().Be(_academicYearViewModel.AcademicYears.FirstOrDefault().Name);
            model.SummaryAcademicYear.RouteName.Should().Be(RouteConstants.ReregisterAcademicYear);
            model.SummaryAcademicYear.ActionText.Should().Be(CheckAndSubmitContent.Change_Action_Link_Text);
            model.SummaryAcademicYear.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterAcademicYear);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(_registrationDetails.ProfileId.ToString());
        }
    }
}
