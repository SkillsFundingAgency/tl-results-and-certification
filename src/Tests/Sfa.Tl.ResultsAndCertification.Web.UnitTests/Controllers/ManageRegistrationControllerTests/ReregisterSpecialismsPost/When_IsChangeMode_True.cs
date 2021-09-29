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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsPost
{
    public class When_IsChangeMode_True : TestSetup
    {
        private readonly string _coreCode = "12345678";
        private ReregisterViewModel _cacheResult;
        private ReregisterCoreViewModel _reregisterCoreViewModel;

        public override void Given()
        {
            _reregisterCoreViewModel = new ReregisterCoreViewModel
            {
                SelectedCoreCode = _coreCode,
                CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } }
            };

            ViewModel = new ReregisterSpecialismViewModel
            {
                SelectedSpecialismCode = "SPL12345",
                IsChangeMode = true,
                PathwaySpecialisms = new PathwaySpecialismsViewModel
                {
                    Specialisms = new List<SpecialismDetailsViewModel>
                    {
                        new SpecialismDetailsViewModel { Id = 11, Code = "SPL12345" },
                        new SpecialismDetailsViewModel { Id = 22, Code = "SPL12346" },
                        new SpecialismDetailsViewModel { Id = 33, Code = "SPL12347" },
                    }
                }
            };

            _cacheResult = new ReregisterViewModel { ReregisterCore = _reregisterCoreViewModel, SpecialismQuestion = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false } };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(_cacheResult);
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, _coreCode).Returns(ViewModel.PathwaySpecialisms);
        }

        [Fact]
        public void Then_CacheUpdated_AsExpected()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x => 
                x.SpecialismQuestion.HasLearnerDecidedSpecialism == true &&
                x.ReregisterSpecialisms.PathwaySpecialisms.Specialisms.SingleOrDefault(s => s.IsSelected).Id == 11));
        }
        
        [Fact]
        public void Then_Redirected_To_ReregisterCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            route.RouteValues.Count.Should().Be(1);
            routeName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
