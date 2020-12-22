using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        private ReregisterViewModel cacheResult;

        public override void Given()
        {
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

            cacheResult = new ReregisterViewModel { SpecialismQuestion = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false } };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
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
