using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismPost
{
    public class When_Success : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private readonly string _coreCode = "12345678";

        public override void Given()
        {
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel
            };

            SelectSpecialismViewModel = new SelectSpecialismViewModel
            {
                SelectedSpecialismCode = "SPL12345",
                PathwaySpecialisms = new PathwaySpecialismsViewModel
                {
                    Specialisms = new List<SpecialismDetailsViewModel>
                    {
                        new SpecialismDetailsViewModel { Id = 11, Code = "SPL12345" },
                        new SpecialismDetailsViewModel { Id = 22, Code = "SPL12346" },
                        new SpecialismDetailsViewModel { Id = 33, Code = "" },
                    }
                }
            };

            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel 
            { 
                PathwayName = "Test Pathway", 
                Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } },
                SpecialismsLookup = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("SPL12345", "Specialism 5"), new KeyValuePair<string, string>("SPL12346", "Specialism 6"), new KeyValuePair<string, string>("SPL12347", "Specialism 7") }
            };
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, _coreCode).Returns(_pathwaySpecialismsViewModel);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationAcademicYear()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
        }

        [Fact]
        public void Then_CacheUpdated_AsExpected()
        {
            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<RegistrationViewModel>(x => x.SelectSpecialisms.PathwaySpecialisms.Specialisms.SingleOrDefault(s => s.IsSelected).Id == 11 && 
                x.SelectSpecialisms.PathwaySpecialisms.SpecialismsLookup.Count() == _pathwaySpecialismsViewModel.SpecialismsLookup.Count()));
        }

        [Fact]
        public void Then_Expected_Method_Is_Called()
        {
            RegistrationLoader.Received(1).GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, _coreCode);
        }
    }
}
